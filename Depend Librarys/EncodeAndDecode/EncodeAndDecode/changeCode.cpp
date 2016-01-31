#include <iostream>
#define _CRT_SECURE_NO_DEPRECATE
int rfidDecode96bit(unsigned char *ucBarCode)	//图书信息解码程序
{

		
	int i, iDatLen, iBarCodeLen;
	//	unsigned char  ucTmpChar;
	unsigned char ucTmpStr[9];
	unsigned char ucWorkStr[15] = "00000000000000";
	// 字符串长度实际为14位字符+结束符' \0'
	union
	{
		unsigned char ucStr[4];
		unsigned int uiDat;
	} unWorkDat;
	iBarCodeLen = (unsigned int)(ucBarCode[0] & 0x0F);
	// 获取压缩前BarCode编码长度
	if (iBarCodeLen < 1 || iBarCodeLen > 14) return (-1);
	if (iBarCodeLen < 8) // 处理1-7字符长度的BarCode
	{
		for (i = 0; i<iBarCodeLen; ++i)
		{
			ucBarCode[i] = ucBarCode[i + 1];
		}
		ucBarCode[iBarCodeLen] = 0;
		return (iBarCodeLen);
	}
	for (i = 0; i<4; ++i)  // 将压缩码的前面(低)4字节取出
	{
		unWorkDat.ucStr[i] = ucBarCode[i];
	}
	unWorkDat.uiDat >>= 4;
	ucWorkStr[iBarCodeLen - 1] = (unsigned char)(unWorkDat.uiDat & 0xFF);
	// 还原最后1位字符，即校验位
	unWorkDat.uiDat >>= 8;
	for (i = 2; i >= 0; --i)  // 解码最前面3位字母或数字
	{
		ucWorkStr[i] = (unsigned char)((unWorkDat.uiDat & 0x3F) + 0x30);
		unWorkDat.uiDat >>= 6;
	}
	ucWorkStr[3] = (unsigned char)(unWorkDat.uiDat & 0x03);
	// 还原第四位数字的低2bit
	for (i = 0; i<4; ++i) // 将压缩码的前面(高)4字节取出
	{
		unWorkDat.ucStr[i] = ucBarCode[i + 4];
	}
	ucWorkStr[3] = (ucWorkStr[3] | ((unsigned char)(unWorkDat.uiDat & 0x03) << 2)) + 0x30;
	// 取出第4位数字的低2bit，还原第4位数字
	unWorkDat.uiDat >>= 2;
	//itoa(unWorkDat.uiDat, (char *)ucTmpStr, 10);
	_itoa_s(unWorkDat.uiDat, (char *)ucTmpStr,9, 10);  //我改成了_itoa_s 加了一个长度参数9，测试下吧
	// 还原BarCode从第5位起除最后1位的2-9位数字
	iDatLen = strlen((char *)ucTmpStr);
	for (i = 0; i<iDatLen; ++i)
	{
		ucWorkStr[i + iBarCodeLen - iDatLen - 1] = ucTmpStr[i];
	}
	for (i = 0; i<iBarCodeLen; ++i)  // 将解码后的BarCode通过参数返回
	{
		ucBarCode[i] = ucWorkStr[i];
	}
	ucBarCode[iBarCodeLen] = 0;
	return (iBarCodeLen);
}

int rfidEncode96bit(unsigned char *ucBarCode)   //图书信息编码程序
{
	int i, iBarCodeLen;
	unsigned char ucCompressedBarCode[4], ucTmpChar;
	unsigned char *pucTmpDat;
	union
	{
		unsigned char ucStr[4];
		unsigned int uiDat;
	} unWorkDat;
	iBarCodeLen = strlen((char *)ucBarCode);
	if (iBarCodeLen < 1 || iBarCodeLen > 14) return (-1);
	if (iBarCodeLen < 8)  // 处理1-7字符长度的BarCode
	{
		for (i = iBarCodeLen; i>0; --i)
		{
			ucBarCode[i] = ucBarCode[i - 1];
		}
		ucBarCode[0] = (unsigned char)iBarCodeLen;
		// 存放原条码长度到低4bit
		return (iBarCodeLen);
	}
	ucTmpChar = ucBarCode[iBarCodeLen - 1];  // 临时存放BarCode校验位
	ucBarCode[iBarCodeLen - 1] = 0;  // 最后一位(即校验位) 清零
	pucTmpDat = &ucBarCode[4];  // 处理压缩后的高4字节
	unWorkDat.uiDat = atoi((char *)pucTmpDat);// 压缩BarCode从第5位起除最后1位的2-9位数字
	unWorkDat.uiDat <<= 2;
	unWorkDat.uiDat |= (((unsigned int)ucBarCode[3] - 0x30) & 0x0C)
		>> 2;  // 将Barcode的第4位数字压缩后的高2bit存入
	for (i = 0; i<4; ++i) // 临时保存压缩后的高4字节
	{
		ucCompressedBarCode[i] = unWorkDat.ucStr[i];
	}
	// 处理压缩后的低4字节
	unWorkDat.uiDat = ((unsigned int)ucBarCode[3] - 0x30)
		& 0x03; // 压缩第4位数字压缩后的低2bit存入
	unWorkDat.uiDat = (unWorkDat.uiDat << 6)
		| ((unsigned int)ucBarCode[0] - 0x30); // 压缩最前面3位字母或数字
	unWorkDat.uiDat = (unWorkDat.uiDat << 6) | ((unsigned int)ucBarCode[1] - 0x30);
	unWorkDat.uiDat = (unWorkDat.uiDat << 6) | ((unsigned int)ucBarCode[2] - 0x30);
	unWorkDat.uiDat = (unWorkDat.uiDat << 8) | ((unsigned int)ucTmpChar);
	// 压缩BarCode最后1位字符，通常为校验码
	unWorkDat.uiDat = (unWorkDat.uiDat << 4) | (unsigned int)iBarCodeLen;
	// 记录原BarCode编码长度
	for (i = 0; i <4; ++i)  // 构成输出压缩后的BarCode
	{
		ucBarCode[i] = unWorkDat.ucStr[i];
		ucBarCode[i + 4] = ucCompressedBarCode[i];
	}
	return (iBarCodeLen);
}

int iRFID_DecodeCCLG(unsigned char *ucBarCode)  //层架信息解码程序
{
	unsigned char bCode[16];
	int i;
	int iPnt = 1;
	int iBarCodeLen = ucBarCode[0]; // 获取压缩前BarCode编码长度
	unsigned char ucVal;
	for (i = 0; i < iBarCodeLen; i++)
	{
		switch (i % 4)
		{
		case 0:
			ucVal = ucBarCode[iPnt++];
			bCode[i] = (ucVal >> 2) + 0x30;
			break;
		case 1:
			bCode[i] = ((ucVal & 3) << 4);
			ucVal = ucBarCode[iPnt++];
			bCode[i] = (bCode[i] | ((ucVal & 0xf0) >> 4)) + 0x30;
			break;
		case 2:
			bCode[i] = ((ucVal & 0xf) << 2);
			ucVal = ucBarCode[iPnt++];
			bCode[i] = (bCode[i] | ((ucVal & 0xc0) >> 6)) + 0x30;
			break;
		case 3:
			bCode[i] = (ucVal & 0x3f) + 0x30;
			break;
		}
	}
	memcpy(ucBarCode, bCode, iBarCodeLen);
	/***
	for (i = 0; i < iBarCodeLen; i++){
		ucBarCode[i] = bCode[i];
	}
	for (i = iBarCodeLen; i < 16; i++){
		ucBarCode[i] = 0x0;
	}
	***/
	return (iBarCodeLen);
}

//当前只有解码器在我的程序中使用，编码器暂时用不着，但源代码依然留着，以备后用。
//参数s为要解码的rfid数据，slen为这个数据的长度，按字节计算。d为编码后得到的结果，函数的返回值为解码后的长度
//，按字节计算。由于解码后最长为15个字节，因此建议调用该函数的C#程序传入一个长度为15的StringBuilder。
//编码结束后，结果放在这个StringBuilder中
extern "C" __declspec(dllexport) int decodeBookRfid(unsigned char *s, int slen, unsigned char *d){
	//std::cout << "begin change" << "\n";
	int maxLengthOfDecodeMsg = 15;
	//unsigned char *rawMsg = (unsigned char *)malloc(slen*sizeof(unsigned char));
	unsigned char *decodeMsg = (unsigned char *)malloc(maxLengthOfDecodeMsg*sizeof(unsigned char));

	int decodedMsgLength = 0;
	for (int i = 0; i < slen; i++){
		decodeMsg[i] = s[i];
	}
	for (int i = slen; i < 15; i++){
		decodeMsg[i] = 0x0;
	}
	decodedMsgLength = rfidDecode96bit(decodeMsg); //调用解码核心函数，并返回解码后的长度
	/***
	std::cout << "Length is:" << decodedMsgLength<<"\n";
	std::cout << "Msg is:" << decodeMsg << "\n";
	***/
	//return the decoded msg
	for (int j = 0; j < decodedMsgLength; j++){
		d[j] = decodeMsg[j];
	}
	for (int j = decodedMsgLength; j < maxLengthOfDecodeMsg; j++){
		d[j] = 0x0;
	}
	free(decodeMsg);
	return decodedMsgLength;	
}

//参数s为要解码的rfid数据，slen为这个数据的长度，按字节计算。d为编码后得到的结果，函数的返回值为解码后的长度
//，按字节计算。由于解码后最长为16个字节，因此建议调用该函数的C#程序传入一个长度为16的StringBuilder。
//编码结束后，结果放在这个StringBuilder中
extern "C" __declspec(dllexport) int decodeShelfRfid(unsigned char *s, int slen, unsigned char *d){
	int maxLengthOfDecodeMsg = 16;
	unsigned char *decodeMsg = (unsigned char *)malloc(maxLengthOfDecodeMsg*sizeof(unsigned char));
	int decodedMsgLength = 0;
	for (int i = 0; i < slen; i++){
		decodeMsg[i] = s[i];
	}
	for (int i = slen; i < maxLengthOfDecodeMsg; i++){
		decodeMsg[i] = 0x0;
	}
	decodedMsgLength = iRFID_DecodeCCLG(decodeMsg); //调用解码核心函数，并返回解码后的长度
	for (int j = 0; j < decodedMsgLength; j++){
		d[j] = decodeMsg[j];
	}
	for (int j = decodedMsgLength; j < maxLengthOfDecodeMsg; j++){
		d[j] = 0x0;
	}
	free(decodeMsg);
	return decodedMsgLength;
}