   M S B A M L   `   `   `  ����  UI$    UI.Views.WrongBookLocationView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � $��	30,30,0,0q�6     � RowDefinitions  5	   
   �� 5
      $
��auto �6   6,   �� 5      $��* �6   6)   .+�5   
   ]� $��0��6   $��0q�6!   .+�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�62   .+�5      �� $��18(�6   当前书架60   �� 5      $	��400��6?   $��20,0,0,0q�6K   $��25��6]   $��18(�6j    �� 
IsReadOnly . ��6   �� *ShelfLocationString+6�   5      �� 5      $��Right=�6   $��Right=�60   $��0,0,20,0q�6L   .+�5      �� $	��100��6O   $��25��6\   $��18(�6i   ��6   �� *" WrongBookLocationCleanAllCommand+.��6w   清除6{   5      5      ]� 5   
   $��1��6   $��	0,10,10,0q�6!   .+�5      �� $��应有图书��6   $	��Top=�6%   $��16(�6:   $	��Top=�6H   $��0q�6`   $��0q�6k   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ allBookOnShelfDataGrid��5      $	��200��5      $��0,5,0,-1q�6)      AutoGenerateColumns .     
IsReadOnly . ��5   ;   �� *OnThisShelfAllBookList+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	序列号��6g      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6t   	   FontSize$	 16Q�6~   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6-   �� *ID+   ClipboardContentBinding 6D   Z� 6~     5      $ 图书名称��6m   $ 3* 6{   $	 16Q�6�   
 6-   �� *
BookName+ 6J   Z� 6�     5      $ 图书索取码��6s   $ 2* 6�   $	 16Q�6�   
 6-   �� *BookAccessCode+ 6P   Z� 6�     5      $ 图书条形码��6q   $ 2* 6�   $	 16Q�6�   
 6-   �� *BookRFIDCode+ 6N   Z� 6�   5       5!      �� 5"      $��未在架图书��6   $��Bottom=�6&   $��16(�6>   $	��Top=�6L   $��0,10,0,0q�6d   $��0q�6v   .��5#        -   5        $ notOnShlefBookDataGrid��5#      $	��200��5$      $	��0,5q�6(    .   . ��5#   ;   �� *NotOnThisShelfBookList+ 5%        5&      $ 	序列号��6g   $ * 6t   $	 16Q�6~   
 6-   �� *ID+ 6D   Z� 6~     5'      $ 图书名称��6m   $ 3* 6{   $	 16Q�6�   
 6-   �� *
BookName+ 6J   Z� 6�     5(      $ 图书索取码��6s   $ 2* 6�   $	 16Q�6�   
 6-   �� *BookAccessCode+ 6P   Z� 6�     5)      $ 图书条形码��6q   $ 2* 6�   $	 16Q�6�   
 6-   �� *BookRFIDCode+ 6N   Z� 6�   5+      5,      5-      5.      5/      