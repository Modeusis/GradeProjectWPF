   M S B A M L   `   `   `  ����Q  MTournamentsApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:TournamentsApplication.ViewTournamentsApplication.View  5      O,clr-namespace:TournamentsApplication.UtilityTournamentsApplication.Utility  5      -    'TournamentsApplication.View.PlayersView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:TournamentsApplication.View     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      4u,clr-namespace:TournamentsApplication.Utility  5        � 	Resources  5      >   8TournamentsApplication.Utility.BoolToVisibilityConverter   	Converter?  9TournamentsApplication.Utility.VisibilityReverseConverter  ReverseConverter0  *TournamentsApplication.Utility.TEConverter 
 TEConv3   -TournamentsApplication.Utility.ImageConverter  IMGConv��%t   &	        &	      &	 &     &	 9       5   
   6&     5   
   6'     5   
   6     5   
   6   5      .��5      � $��150,20,150,10q�6    � RowDefinitions 5   
   �� 5      $��80 �6   �� 5      �� 5      $��60 �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5      �� 5      $	��150 �6#   .+�5      � .+�5      �� $��0,0,10,0q�6    J� Opacity$	 0.6Q�6.    �� RadiusX$ 10��6<    �� RadiusY$ 10��6I   ��5      �� 5        �� ImageSource$: 4pack://application:,,,/Resources/Images/teamback.jpg��6%    j� Stretch$ UniformToFill=�6h   5"      �� 5#        FavTextBlock#��[ 6   $��Players��6B   5$      �� 5%      $��20(�6X   $��1��6f   $��2��6v     UnlimitedTextBox#��[ 6�   ��6   �� *CurrentText+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5&      $��2��6   $	��120��6%   $��18(�61   $��50��6g     
HeadButton#��[ 6s   $��By name��6�   ��6?   �� *CurrentOrderCommand+6�   5'      �� 5(   
   $��1��6   $��0,10,0,0q�6     TeamPgListBoxItem#��[ 5)   
     TeamPlayersListBox#��[ 5*   
   $��Stretch=�5+   
   $
��Auto=�5.   
   $��Disabled=�5/   
   ��5,   
   �� *SortedPlayers+	 �� SelectedItem	 5-   
   �� *SelectedItem+��50      �� 51      .R�52      �� 
 �� Orientation$
 Vertical=�6!   53      ��55      �� 56      .R�57      � $��1��6   $��Center=�6,   $��60��6G   .+�58      � $
��10,5q�6    59      �� 5:   "   $��60 �63   �� 5;   "   $
��Auto �63   �� 5<   "   $
��Auto �63   �� 5=   "   $��* �63   .+�5?      �� $
��10,0q�6(   $ 1Q�66   $��0��6B   $ 5��6S   $ 5��6_   ��5@   "   �� 5A   &   $ Uniform=�6S    61   �� *	PlayerImg+6S   5C      �� 5D      $��1��6(   #��[ 69   ��5E      �� *
PlayerName+�� 5F      $��2��6)   $
 
Horizontal=�69   $��20,0,0,0q�6R   .+�5G   "   �� $��18(�6,   $��Black"�6:    	 PlayerTeamPgStandardBlock#��[	 6M   ��5H      �� *
Position+5I      �� 5K       �� CornerRadius$ 5��6%   $��0,0,10,0q�66   $��Right=�6H   $��Center=�6d   $��3��6  ���#121212���#383838$��1q�6A  y�6   �� *DataContext.IsAdmin+ �� RelativeSource ��  �� AncestorType ListBox �� 	Converter# [  ._�5L   "   �� $
 
Horizontal=�6-   $��Center=�6G   $��Right=�6b   .+�5M   &   �� $��0q�6-   $��40��69    
 IconDeleteButtonStyle#��[
 5N      .��5O   *   ��   DeleteIconImageStyle#��[ 60   $8��2pack://application:,,,/Resources/Images/delete.png��6^    J� InputBindings 5P   *   d� 5Q   .    d� Gesture$ 	LeftClicka�6;    �� Command 5R   3   �� *!DataContext.DeletePlayerCommand+ ��  ListBox �� CommandParameter 5S   3   �� 5U   '   5V   #   5W      5X       5Y      d� 5Z      $ 	LeftClicka�6+    5[   #   �� *!DataContext.ToPlayerPageCommand+ ��  ListBox 5\   #   �� 5^      5_      5a      � 5b   
   $��2��6   .+�5c      �� ���#121212$ 5��6*   ���#383838$��1q�5d   #   $��Center=�5e      $��Center=�5f      $��1��6*   ._�5g      � $��5q�6    5h      �� 5i      �� 5j      $
��Auto �6+   �� 5k      $
��Auto �6+   �� 5l      .+�5n      �� $��0��6     ContentButtonStyle#��[ 6U   ��6-   �� *PreviousPageCommand+.��5o      �� $��20��6    $;��5pack://application:,,,/Resources/Images/leftArrow.png��6+   5p      �� 5q      $��1��6    $��10,0,0,0q�60     H2TextBlock#��[ 6B   $��Page: ��6g   �� 5r      $��2��6    $��0,0,10,0q�61   #��[ 6C   ��6h   �� *CurrentPlayersPage+�� 5s      $��3��6A   #��[ 6Q   ��6   �� *NextPageCommand+.��5t      �� $��20��6    $<��6pack://application:,,,/Resources/Images/rightArrow.png��6+   5u      5v      5w      5x      5y      5z      