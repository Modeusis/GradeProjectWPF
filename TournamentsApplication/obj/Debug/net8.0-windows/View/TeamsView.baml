   M S B A M L   `   `   `  ����Q  MTournamentsApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:TournamentsApplication.ViewTournamentsApplication.View  5      O,clr-namespace:TournamentsApplication.UtilityTournamentsApplication.Utility  5      +    %TournamentsApplication.View.TeamsView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:TournamentsApplication.View     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      4u,clr-namespace:TournamentsApplication.Utility  5        � 	Resources  5      >   8TournamentsApplication.Utility.BoolToVisibilityConverter   	Converter?  9TournamentsApplication.Utility.VisibilityReverseConverter  ReverseConverter0  *TournamentsApplication.Utility.TEConverter 
 TEConv3   -TournamentsApplication.Utility.ImageConverter  IMGConv��%t   &	        &	      &	 &     &	 9       5   
   6&     5   
   6'     5   
   6     5   
   6   5      .��5      � $��150,20,150,10q�6    � RowDefinitions 5   
   �� 5      $��80 �6   �� 5      �� 5      $��60 �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5      �� 5      $	��150 �6#   .+�5      � .+�5      �� $��0,0,10,0q�6    J� Opacity$	 0.6Q�6.    �� RadiusX$ 10��6<    �� RadiusY$ 10��6I   ��5      �� 5        �� ImageSource$: 4pack://application:,,,/Resources/Images/teamback.jpg��6%    j� Stretch$ UniformToFill=�6h   5"      �� 5#        FavTextBlock#��[ 6   $��Teams��6B   5$      �� 5%      $��20(�6X   $��1��6f   $��2��6v     UnlimitedTextBox#��[ 6�   ��6   �� *CurrentText+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5&      $��2��6   $	��120��6%   $��18(�61   $��50��6g     
HeadButton#��[ 6s   $��
World rank��6�   ��6?   �� *CurrentOrderCommand+6�   5'      �� 5(   
   $��1��6   $��0,10,0,0q�6     TeamPgListBoxItem#��[ 5)        TeamPlayersListBox#��[ 5*      $��Stretch=�5+      $
��Auto=�5.      $��Disabled=�5/      ��5,      �� *SortedTeams+	 �� SelectedItem	 5-      �� *SelectedItem+��50      �� 51      .R�52      �� 
 �� Orientation$
 Vertical=�6!   53      ��55      �� 56      .R�57      � $��1��6   $��Center=�6,   $��60��6G   .+�58      � $
��10,5q�6    59      �� 5:   "   $��60 �63   �� 5;   "   $
��Auto �63   �� 5<   "   $
��Auto �63   �� 5=   "   $��* �63   .+�5?      �� $
��10,0q�6%   $��45��63   $��45��6>   ��5@   "   
� 5A   &    
� Center$ 0.5,0.5&�6:    
� GradientOrigin$ 0.5,0.5&�6K   .��5B   *   �  � Color$ #ff5500��67    � Offset$ 0Q�6G   � 5C   *   $ Transparent��67   $ 1Q�6K   5D   '   ._�5F   "   �� $��Uniform=�6(   ��6:   �� *
TeamLogo+5G      �� 5H      $��1��6(   #��[ 68   ��5I   !   �� *
TeamName+�� 5J      $��2��6)   $
 
Horizontal=�69   $��20,0,0,0q�6R   .+�5K   "   �� #��[ 6,   $��#��5L      �� 5M   "   #��[ 6,   ��5N      �� *WorldRanking+5O      �� 5Q       �� CornerRadius$ 5��6%   $��0,0,10,0q�66   $��Right=�6H   $��Center=�6d   $��3��6  ���#121212���#383838$��1q�6A  y�6   �� *DataContext.IsAdmin+ �� RelativeSource ��  �� AncestorType ListBox �� 	Converter# [  ._�5R   "   �� $
 
Horizontal=�6-   $��Center=�6G   $��Right=�6b   .+�5S   &   �� $��0q�6-   $��40��69    	 IconDeleteButtonStyle#��[	 5T   !   .��5U   *   ��  
 DeleteIconImageStyle#��[
 60   $8��2pack://application:,,,/Resources/Images/delete.png��6^    J� InputBindings 5V   *   d� 5W   .    d� Gesture$ 	LeftClicka�6;    �� Command 5X   7   �� *DataContext.DeleteTeamCommand+ ��  ListBox �� CommandParameter 5Y   7   �� 5[   '   5\   #   5]      5^       5_      d� 5`      $ 	LeftClicka�6+    5a   '   �� *DataContext.ToTeamPageCommand+ ��  ListBox 5b   '   �� 5d      5e      5g      � 5h   
   $��2��6   .+�5i      �� ���#121212$ 5��6*   ���#383838$��1q�5j   '   $��Center=�5k      $��Center=�5l      $��1��6.   ._�5m      � $��5q�6    5n      �� 5o      �� 5p      $
��Auto �6+   �� 5q      $
��Auto �6+   �� 5r      .+�5t      �� $��0��6     ContentButtonStyle#��[ 6U   ��6-   �� *PreviousPageCommand+.��5u      �� $��20��6    $;��5pack://application:,,,/Resources/Images/leftArrow.png��6+   5v      �� 5w      $��1��6    $��10,0,0,0q�60     H2TextBlock#��[ 6B   $��Page: ��6g   �� 5x      $��2��6    $��0,0,10,0q�61   #��[ 6C   ��6h   �� *CurrentTeamsPage+�� 5y      $��3��6A   #��[ 6Q   ��6   �� *NextPageCommand+.��5z      �� $��20��6    $<��6pack://application:,,,/Resources/Images/rightArrow.png��6+   5{      5|      5}      5~      5      5�      