   M S B A M L   `   `   `  ����  	WootAlert    WootAlert.SettingsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   �   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6�     � Title$  Settings��6	    � 
ResizeMode$ NoResize=�6    � Topmost .   � WindowStartupLocation$ CenterScreen=�6@    � WindowStyle$ 
ToolWindow=�6e   $	��322��6  $	��493��6   .��5      � .+�5   
   �� -   5        $��	checkBox1��5   i   $��Play Audio Alerts��6   $��16��6/   $
��Left=�6;   $��	12,12,0,0q�6V   $	��Top=�6z   6�   �� 5   
   -   5        $��	checkBox2��5   q   $��Start when Windows Starts��6   $��16��67   $
��Left=�6C   $��	12,92,0,0q�6^   $	��Top=�6�   �� 5	   
   -   5        $��
saveButton��5	   \   $
��Save��6   $��23��6    $
��Left=�6,   $��303,248,0,0q�6G   $	��Top=�6n   $��75��6�    �� 	IsDefault . 6�   �� 5
   
   -   5        $��cancelButton��5
   ^   $��Cancel��6   $��23��6"   $
��Left=�6.   $��384,248,0,0q�6I   $	��Top=�6r   $��75��6�    �� IsCancel . 6�   �� 5   
   -   5        $��label1��5   j   $��Woot-Off Refresh Time��6   $��28��60   $
��Left=�6<   $��	12,58,0,0q�6W   $	��Top=�6x   $	��204��6�   �� 5   
   -   5        $��label2��5   f   $��Woot Refresh Time��6   $��28��6,   $
��Left=�68   $��	12,34,0,0q�6S   $	��Top=�6t   $	��204��6�   �� 5   
   -   5        $��label3��5      $��28��6D   $��Right=�6P   $��	0,34,48,0q�6l   $	��Top=�6�   ��6   ��  �� Path$ Value� �� ElementName$ slider1��6�   �� 5   
   -   5        $��label4��5      $��28��6D   $��Right=�6P   $��	0,58,48,0q�6l   $	��Top=�6�   ��6   �� $ Value�$ slider2��6�   �� 5   
   -	   5        $��label5��5   M   $	��min��6   $
��Left=�6   $��
426,34,0,0q�69   $��28��6[   $	��Top=�6g   �� 5   
   -
   5        $��label6��5   Z   $	��sec��6   $��28��6   $
��Left=�6*   $��429,0,0,197q�6E   $��Bottom=�6h   �� 5   
   -   5        $��slider1��5   L   $��23��6   $
��Left=�6   $��
161,34,0,0q�68   $	��Top=�6[   $	��226��6s   	 �� IsSnapToTickEnabled	 . 
 � Maximum$
 60Q�6�    � Minimum$ 10Q�6�    �� TickPlacement$ BottomRight=�6�    �� TickFrequency$ 10Q�6�   �� 5   
   -   5        $��slider2��5   L   $��23��6   $
��Left=�6   $��
161,63,0,0q�68   $	��Top=�6[   $	��226��6s   	 . $ 10Q�6�   $ 10Q�6�   $	
 100Q�6�   $ BottomRight=�6�   5      5      