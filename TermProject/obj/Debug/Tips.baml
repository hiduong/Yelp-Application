   M S B A M L   `   `   `  ����F  BTermProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TermProjectTermProject  5   	       TermProject.Tips   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TermProject     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$
 Tips��5	   	   $	��650��6   $��1000         ��6#   .��5      � ���	#FF96DBB1 � ColumnDefinitions 5   
   �� 5      $
��128* �6   �� 5      $
��153* �6    � RowDefinitions 5   
   �� 5      $	��66* �6   �� 5      $
��304* �6   �� 5      $��6* �6   �� 5      $
��245* �6   .+�5   
   �� -   5         ��Name$ AddTipButton��5      $��Add Tip��6'   $��0,20,6.333,9q�69   $��Arial)�6O   $��1��6b   $��Right=�6r   $	��103��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TipsGrid��5      $��13,68,114.333,0q�6%   $��3��6>   $��2��6O   $	��323��6c   $	��Top=�6p   $��1��6�   �� 5   
   -   5         ��Name$ BusinessTipsLabel��5      $��Business Tips��6+   $
��Left=�6C   $��8,41,0,0q�6^   $	��Top=�6p   $��Arial)�6�   $��14(�6�   $
��Bold"�6�   $��1��6�   �� 5   
   -   5         ��Name$ TextBox��5      $��94��6#   $��14,12,114.333,0q�6/    �� TextWrapping$
 Wrap=�6H   $��<<tip text>>��6\   $	��Top=�6|   $��2��6�   $��2��6�   $��14(�6�   $��Arial)�6�   �� 5   
   -   5        $ AddTipButton_Copy��5      $��Like Tip��6,   $��
0,78,6,188q�6?   $��Arial)�6S   $��1��6f   $��Right=�6v   $	��103��6�   $��1��6�   �� 5   
   -   5        $ FriendsLabel��5      $(��"Friends who reviewed this business��6&   $
��Left=�6S   $��8,81,0,0q�6n   $	��Top=�6�   $��Arial)�6�   $��14(�6�   $
��Bold"�6�   $��3��6�     5   
   -   5        $ FriendsGrid��5      $��13,106,114.333,7.667q�6(   $��2��6F   $��3��6Z   5      5      