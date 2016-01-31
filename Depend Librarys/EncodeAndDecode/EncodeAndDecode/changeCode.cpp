#include <iostream>
#define _CRT_SECURE_NO_DEPRECATE
int rfidDecode96bit(unsigned char *ucBarCode)	//ͼ����Ϣ�������
{

		
	int i, iDatLen, iBarCodeLen;
	//	unsigned char  ucTmpChar;
	unsigned char ucTmpStr[9];
	unsigned char ucWorkStr[15] = "00000000000000";
	// �ַ�������ʵ��Ϊ14λ�ַ�+������' \0'
	union
	{
		unsigned char ucStr[4];
		unsigned int uiDat;
	} unWorkDat;
	iBarCodeLen = (unsigned int)(ucBarCode[0] & 0x0F);
	// ��ȡѹ��ǰBarCode���볤��
	if (iBarCodeLen < 1 || iBarCodeLen > 14) return (-1);
	if (iBarCodeLen < 8) // ����1-7�ַ����ȵ�BarCode
	{
		for (i = 0; i<iBarCodeLen; ++i)
		{
			ucBarCode[i] = ucBarCode[i + 1];
		}
		ucBarCode[iBarCodeLen] = 0;
		return (iBarCodeLen);
	}
	for (i = 0; i<4; ++i)  // ��ѹ�����ǰ��(��)4�ֽ�ȡ��
	{
		unWorkDat.ucStr[i] = ucBarCode[i];
	}
	unWorkDat.uiDat >>= 4;
	ucWorkStr[iBarCodeLen - 1] = (unsigned char)(unWorkDat.uiDat & 0xFF);
	// ��ԭ���1λ�ַ�����У��λ
	unWorkDat.uiDat >>= 8;
	for (i = 2; i >= 0; --i)  // ������ǰ��3λ��ĸ������
	{
		ucWorkStr[i] = (unsigned char)((unWorkDat.uiDat & 0x3F) + 0x30);
		unWorkDat.uiDat >>= 6;
	}
	ucWorkStr[3] = (unsigned char)(unWorkDat.uiDat & 0x03);
	// ��ԭ����λ���ֵĵ�2bit
	for (i = 0; i<4; ++i) // ��ѹ�����ǰ��(��)4�ֽ�ȡ��
	{
		unWorkDat.ucStr[i] = ucBarCode[i + 4];
	}
	ucWorkStr[3] = (ucWorkStr[3] | ((unsigned char)(unWorkDat.uiDat & 0x03) << 2)) + 0x30;
	// ȡ����4λ���ֵĵ�2bit����ԭ��4λ����
	unWorkDat.uiDat >>= 2;
	//itoa(unWorkDat.uiDat, (char *)ucTmpStr, 10);
	_itoa_s(unWorkDat.uiDat, (char *)ucTmpStr,9, 10);  //�Ҹĳ���_itoa_s ����һ�����Ȳ���9�������°�
	// ��ԭBarCode�ӵ�5λ������1λ��2-9λ����
	iDatLen = strlen((char *)ucTmpStr);
	for (i = 0; i<iDatLen; ++i)
	{
		ucWorkStr[i + iBarCodeLen - iDatLen - 1] = ucTmpStr[i];
	}
	for (i = 0; i<iBarCodeLen; ++i)  // ��������BarCodeͨ����������
	{
		ucBarCode[i] = ucWorkStr[i];
	}
	ucBarCode[iBarCodeLen] = 0;
	return (iBarCodeLen);
}

int rfidEncode96bit(unsigned char *ucBarCode)   //ͼ����Ϣ�������
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
	if (iBarCodeLen < 8)  // ����1-7�ַ����ȵ�BarCode
	{
		for (i = iBarCodeLen; i>0; --i)
		{
			ucBarCode[i] = ucBarCode[i - 1];
		}
		ucBarCode[0] = (unsigned char)iBarCodeLen;
		// ���ԭ���볤�ȵ���4bit
		return (iBarCodeLen);
	}
	ucTmpChar = ucBarCode[iBarCodeLen - 1];  // ��ʱ���BarCodeУ��λ
	ucBarCode[iBarCodeLen - 1] = 0;  // ���һλ(��У��λ) ����
	pucTmpDat = &ucBarCode[4];  // ����ѹ����ĸ�4�ֽ�
	unWorkDat.uiDat = atoi((char *)pucTmpDat);// ѹ��BarCode�ӵ�5λ������1λ��2-9λ����
	unWorkDat.uiDat <<= 2;
	unWorkDat.uiDat |= (((unsigned int)ucBarCode[3] - 0x30) & 0x0C)
		>> 2;  // ��Barcode�ĵ�4λ����ѹ����ĸ�2bit����
	for (i = 0; i<4; ++i) // ��ʱ����ѹ����ĸ�4�ֽ�
	{
		ucCompressedBarCode[i] = unWorkDat.ucStr[i];
	}
	// ����ѹ����ĵ�4�ֽ�
	unWorkDat.uiDat = ((unsigned int)ucBarCode[3] - 0x30)
		& 0x03; // ѹ����4λ����ѹ����ĵ�2bit����
	unWorkDat.uiDat = (unWorkDat.uiDat << 6)
		| ((unsigned int)ucBarCode[0] - 0x30); // ѹ����ǰ��3λ��ĸ������
	unWorkDat.uiDat = (unWorkDat.uiDat << 6) | ((unsigned int)ucBarCode[1] - 0x30);
	unWorkDat.uiDat = (unWorkDat.uiDat << 6) | ((unsigned int)ucBarCode[2] - 0x30);
	unWorkDat.uiDat = (unWorkDat.uiDat << 8) | ((unsigned int)ucTmpChar);
	// ѹ��BarCode���1λ�ַ���ͨ��ΪУ����
	unWorkDat.uiDat = (unWorkDat.uiDat << 4) | (unsigned int)iBarCodeLen;
	// ��¼ԭBarCode���볤��
	for (i = 0; i <4; ++i)  // �������ѹ�����BarCode
	{
		ucBarCode[i] = unWorkDat.ucStr[i];
		ucBarCode[i + 4] = ucCompressedBarCode[i];
	}
	return (iBarCodeLen);
}

int iRFID_DecodeCCLG(unsigned char *ucBarCode)  //�����Ϣ�������
{
	unsigned char bCode[16];
	int i;
	int iPnt = 1;
	int iBarCodeLen = ucBarCode[0]; // ��ȡѹ��ǰBarCode���볤��
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

//��ǰֻ�н��������ҵĳ�����ʹ�ã���������ʱ�ò��ţ���Դ������Ȼ���ţ��Ա����á�
//����sΪҪ�����rfid���ݣ�slenΪ������ݵĳ��ȣ����ֽڼ��㡣dΪ�����õ��Ľ���������ķ���ֵΪ�����ĳ���
//�����ֽڼ��㡣���ڽ�����Ϊ15���ֽڣ���˽�����øú�����C#������һ������Ϊ15��StringBuilder��
//��������󣬽���������StringBuilder��
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
	decodedMsgLength = rfidDecode96bit(decodeMsg); //���ý�����ĺ����������ؽ����ĳ���
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

//����sΪҪ�����rfid���ݣ�slenΪ������ݵĳ��ȣ����ֽڼ��㡣dΪ�����õ��Ľ���������ķ���ֵΪ�����ĳ���
//�����ֽڼ��㡣���ڽ�����Ϊ16���ֽڣ���˽�����øú�����C#������һ������Ϊ16��StringBuilder��
//��������󣬽���������StringBuilder��
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
	decodedMsgLength = iRFID_DecodeCCLG(decodeMsg); //���ý�����ĺ����������ؽ����ĳ���
	for (int j = 0; j < decodedMsgLength; j++){
		d[j] = decodeMsg[j];
	}
	for (int j = decodedMsgLength; j < maxLengthOfDecodeMsg; j++){
		d[j] = 0x0;
	}
	free(decodeMsg);
	return decodedMsgLength;
}