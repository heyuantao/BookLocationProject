   M S B A M L   `   `   `  ����  UI#    UI.Views.BookLocationShowView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5
      � $��	30,30,0,0q�6     � ColumnDefinitions  5   
   �� 5      $��1091* �6   �� 5      $
��180* �6    � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)   .+�5   
   ]� $��0��6   $��2��6!   $��0,0,0,10q�65   .+�5      �� $
��Left=�6    �� Orientation$ 
Horizontal=�6/   $
��Left=�6H   .+�5      �� $��18(�6d   $��25��6s   $��0,0,15,0q�6�   ��6   ��  �� ElementName$ bookLocationTextBlock�� �� Path$ ActualWidth�图书名称6�   �� 5      $��25��64   $��18(�6A   $	��230��6O    �� 
IsReadOnly . $��0,0,15,0q�6m   ��6   �� *
BookName+6m   �� 5      $��25��6c   $��18(�6p   $��0,0,15,0q�6~   ��6   �� $ bookLocationTextBlock��$ ActualWidth�图书索取号6�   �� 5      $��25��6:   $��18(�6G   $	��230��6U    . $��0,0,15,0q�6s   ��6   �� *BookAccessCode+6s   5      �� 5      $��Right=�6   $ Vertical=�61   $��Right=�6H   .+�5      �� $��25��6J   $��18(�6W   $��90��6e   $��Right=�6p   $��0,0,10,0q�6�   ��6   �� *BookLocationShowClearCommand+.��6�   清除6�   5      5      �� 5   
   $��1��6   $ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5       �� -   5         ��Name$ bookLocationTextBlock��5       $��25��67   $��18(�6D   $��0,0,15,0q�6R   图书存放位置6n   �� 5!      $��25��64   $��18(�6A   $	��600��6O    . $��0,0,15,0q�6m   ��6   �� *BookLocation+6m   5#      � 5$   
   $��2��6   $��2��6   $��0,0,0,10q�60     5%      �� 5&      $��2* �6#   60   �� 5'      $��8* �6#   60   .+�5)      (� $��0��6    (� Stretch$ Uniform=�6'    (� StretchDirection$
 Both=�69   $	��Top=�6Q   .��5*      �� |�. $��10q�6g   ��6$   �� *LibraryMapService.OneShelfMap+6g   5+      (� 5,      $��1��6   $ Uniform=�6&   $
 Both=�68   $��0,0,12,0q�6P   $	��Top=�6b   .��5-      �� |�. $��10q�6k   ��6$   �� *#!LibraryMapService.LibraryShelfMap+6k   5/      51      52      53      