   M S B A M L   `   `   `  ����   	WootAlert    WootAlert.SettingsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  Settings�� � 
ResizeMode$ NoResize=� � Topmost .   � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=�$	��322��$	��493��.��� .+��� -   $��	checkBox1��$��Play Audio Alerts��$��16��$
��Left=�$��	12,12,0,0q�$	��Top=��� -   $��	checkBox2��$��Start when Windows Starts��$��16��$
��Left=�$��	12,92,0,0q�$	��Top=��� -   $��
saveButton��$
��Save��$��23��$
��Left=�$��303,248,0,0q�$	��Top=�$��75�� �� 	IsDefault . �� -   $��cancelButton��$��Cancel��$��23��$
��Left=�$��384,248,0,0q�$	��Top=�$��75�� �� IsCancel . �� -   $��label1��$��Woot-Off Refresh Time��$��28��$
��Left=�$��	12,58,0,0q�$	��Top=�$	��204���� -   $��label2��$��Woot Refresh Time��$��28��$
��Left=�$��	12,34,0,0q�$	��Top=�$	��204���� -   $��label3��$��28��$��Right=�$��	0,34,48,0q�$	��Top=�����  �� Path$ Value� �� ElementName$ slider1���� -   $��label4��$��28��$��Right=�$��	0,58,48,0q�$	��Top=����� $ Value�$ slider2���� -	   $��label5��$	��min��$
��Left=�$��
426,34,0,0q�$��28��$	��Top=��� -
   $��label6��$	��sec��$��28��$
��Left=�$��429,0,0,197q�$��Bottom=��� -   $��slider1��$��23��$
��Left=�$��
161,34,0,0q�$	��Top=�$	��226��	 �� IsSnapToTickEnabled	 . 
 � Maximum$
 60Q� � Minimum$ 10Q� �� TickPlacement$ BottomRight=� �� TickFrequency$ 10Q��� -   $��slider2��$��23��$
��Left=�$��
161,63,0,0q�$	��Top=�$	��226��	 . $ 10Q�$ 10Q�$	
 100Q�$ BottomRight=�