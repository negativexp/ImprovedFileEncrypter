   M S B A M L   `   `   `  ����C  ?IFE(6.0), Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:IFE_6._0_	IFE_6._0_  5   	       IFE_6._0_.OptionsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:IFE_6._0_     5   	     � Title$  OptionsWindow��5   	   $	��225��6   $	��387��6,    � 
ResizeMode$ NoResize=�5	   	   .��5
      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   ]� $��0��6   $
��True��6!   .+�5      �� $��Buffer size:��6   $
��Left=�6,   $��Center=�6B   $��10, 0, 5, 0q�6]   �� 5      $��Bytes��6   $��Right=�6%   $��Center=�6<   $��5, 0, 10, 0q�6W   �� 5      -   5         ��Name$ TextBoxBuffer��5      $
��4096��6-   $��Center=�69   $��
5, 0, 5, 0q�6T   5      � 5   
   $��1��6    5      �� 5      �� 5      .+�5      ]� $
��True��6   .+�5      �� $��
Loaction: ��6   $
��Left=�6.   $��Center=�6D   $��10, 0, 5, 0q�6_   �� 5      -   5         ��Name$ ButtonBrowse��5      $��Right=�6/   $��Center=�6F   $��5, 0, 10, 0q�6a   $��Browse��6v   $��75��6�   6�   �� 5      -   5        $ TextBoxLoaction��5      $��Center=�63   $��5, 0, 10, 0q�6N   ${�False��6c   5       ]� 5!      $��1��6   .+�5"      �� $��
Loaction: ��6   $
��Left=�6.   $��Center=�6D   $��10, 0, 5, 0q�6_   $y�Hidden=�6t   �� 5#      -   5        $&�� Same folder where input file is.��5#      $��Center=�6F   $��5, 0, 10, 0q�6a    d� 	IsChecked$
 TrueY�6v   6�   5$      5%      � 5'   
   $��2��6   .+�5(      �� -   5        $��75��5(      $��Save and Exit��6    $��Bottom=�68   $��Right=�6S   $��5q�6o   6z   5)      5+      5,      