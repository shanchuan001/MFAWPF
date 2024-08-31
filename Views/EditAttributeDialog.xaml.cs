﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MFAWPF.Controls;
using MFAWPF.Utils;
using MFAWPF.ViewModels;
using HandyControl.Controls;
using HandyControl.Data;
using Newtonsoft.Json;
using WPFLocalizeExtension.Extensions;
using Attribute = MFAWPF.Utils.Attribute;
using ComboBox = HandyControl.Controls.ComboBox;
using ScrollViewer = System.Windows.Controls.ScrollViewer;
using TextBlock = System.Windows.Controls.TextBlock;
using TextBox = System.Windows.Controls.TextBox;

namespace MFAWPF.Views;

public partial class EditAttributeDialog : CustomWindow
{
    public Attribute Attribute { get; private set; }
    private bool IsEdit = true;
    private UIElement? Control;
    private CustomWindow? ParentDialog;

    public EditAttributeDialog(CustomWindow? parentDialog, Attribute? attribute = null, bool isEdit = false) :
        base()
    {
        InitializeComponent();
        ParentDialog = parentDialog;
        IsEdit = isEdit;
        Attribute = new Attribute()
        {
            Key = attribute?.Key, Value = attribute?.Value
        };
        var Types = new List<string>()
        {
            "recognition",
            "action",
            "next",
            "is_sub",
            "inverse",
            "enabled",
            "timeout",
            "timeout_next",
            "times_limit",
            "runout_next",
            "pre_delay",
            "post_delay",
            "pre_wait_freezes",
            "post_wait_freezes",
            "focus",
            "focus_tip",
            "focus_tip_color",
            "expected",
            "only_rec",
            "labels",
            "model",
            "target",
            "target_offset",
            "begin",
            "begin_offset",
            "end",
            "end_offset",
            "duration",
            "key",
            "input_text",
            "package",
            "custom_recognition",
            "custom_recognition_param",
            "custom_action",
            "custom_action_param",
            "order_by",
            "index",
            "method",
            "count",
            "green_mask",
            "detector",
            "ratio",
            "template",
            "roi",
            "threshold",
            "lower",
            "upper",
            "connected"
        };


        typeComboBox.ItemsSource = Types;
        if (attribute?.Key != null)
        {
            typeComboBox.SelectedValue = attribute.Key;
            SwitchByType(attribute.Key, attribute.Value);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        Attribute.Key = typeComboBox.SelectedValue.ToString();
        if (Attribute.Key != null)
            ReadValue(Attribute.Key);
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void SwitchString(object? defaultValue)
    {
        AttributePanel.Children.Clear();

        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        dynamicGrid.Children.Add(textBlock);

        AttributePanel.Children.Add(dynamicGrid);

        TextBox textBox = new TextBox()
        {
            Style = FindResource("TextBoxExtend") as Style, Margin = new Thickness(0, 5, 0, 15),
            Text = defaultValue != null ? defaultValue.ToString() : ""
        };
        Control = textBox;
        AttributePanel.Children.Add(Control);
    }

    private void SwitchBool(object? defaultValue)
    {
        AttributePanel.Children.Clear();
        // 创建一个新的 Grid
        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        dynamicGrid.Children.Add(textBlock);

        AttributePanel.Children.Add(dynamicGrid);
        var Types = new List<bool>()
        {
            true,
            false
        };
        ComboBox comboBox = new ComboBox()
        {
            Margin = new Thickness(0, 5, 0, 15), ItemsSource = Types
        };
        if (defaultValue == null)
            comboBox.SelectedIndex = 0;
        else comboBox.SelectedValue = defaultValue;
        Control = comboBox;
        AttributePanel.Children.Add(Control);
    }

    private void SwitchCombo(string key, object? defaultValue)
    {
        AttributePanel.Children.Clear();

        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        dynamicGrid.Children.Add(textBlock);

        AttributePanel.Children.Add(dynamicGrid);
        var Types = new List<string>();
        switch (key)
        {
            case "recognition":
                Types = new List<string>()
                {
                    "DirectHit",
                    "TemplateMatch",
                    "FeatureMatch",
                    "ColorMatch",
                    "OCR",
                    "NeuralNetworkClassify",
                    "NeuralNetworkDetect",
                    "Custom"
                };
                break;
            case "action":
                Types = new List<string>()
                {
                    "DoNothing",
                    "Click",
                    "Swipe",
                    "Key",
                    "Text",
                    "StartApp",
                    "StopApp",
                    "StopTask",
                    "Custom"
                };
                break;
            case "order_by":
                Types = new List<string>()
                {
                    "Horizontal",
                    "Vertical",
                    "Score",
                    "Random",
                    "Area",
                    "Length"
                };
                break;
            case "detector":
                Types = new List<string>()
                {
                    "SIFT",
                    "KAZE",
                    "AKAZE",
                    "BRISK",
                    "ORB"
                };
                break;
        }

        ComboBox comboBox = new ComboBox()
        {
            Margin = new Thickness(0, 5, 0, 15), ItemsSource = Types
        };
        if (defaultValue == null)
            comboBox.SelectedIndex = 0;
        else comboBox.SelectedValue = defaultValue;
        Control = comboBox;
        AttributePanel.Children.Add(comboBox);
    }

    private void SwitchAutoList(object? defaultValue)
    {
        AttributePanel.Children.Clear();

        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        Button button = new Button
        {
            Style = FindResource("textBoxButton") as Style,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 15, Padding = new Thickness(0),
            Height = 15
        };
        button.BindLocalization("Add", Button.ToolTipProperty);
        Binding foregroundBinding = new Binding
        {
            Source = Application.Current.Resources,
            Path = new PropertyPath("GrayColor4")
        };
        button.SetBinding(Button.ForegroundProperty, foregroundBinding);

        var selectAllGeometry = (Geometry)FindResource("AddGeometry");
        IconElement.SetGeometry(button, selectAllGeometry);

        button.Click += AddAutoAttribute;
        dynamicGrid.Children.Add(textBlock);
        dynamicGrid.Children.Add(button);

        AttributePanel.Children.Add(dynamicGrid);

        Border border = new Border
        {
            Height = 120,
            Background = Brushes.White,
            CornerRadius = new CornerRadius(8),
            Margin = new Thickness(5, 5, 5, 0)
        };

        ScrollViewer scrollViewer = new ScrollViewer();

        StackPanel stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        Control = stackPanel;
        scrollViewer.Content = Control;
        border.Child = scrollViewer;

        AttributePanel.Children.Add(border);

        if (defaultValue is bool b)
        {
            if (b)
                AddAutoAttribute(stackPanel, true);
        }
        else if (defaultValue is string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
                AddAutoAttribute(stackPanel, s);
        }
        else if (defaultValue is List<string> ls)
        {
            foreach (var VARIABLE in ls)
            {
                AddAutoAttribute(stackPanel, VARIABLE);
            }
        }
        else if (defaultValue is List<int> li)
        {
            foreach (var VARIABLE in li)
            {
                AddAutoAttribute(stackPanel, VARIABLE);
            }
        }
    }

    private void SwitchList(object? defaultValue)
    {
        AttributePanel.Children.Clear();

        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        Button button = new Button
        {
            Style = FindResource("textBoxButton") as Style,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 15, Padding = new Thickness(0),
            Height = 15
        };
        button.BindLocalization("Add", Button.ToolTipProperty);
        Binding foregroundBinding = new Binding
        {
            Source = Application.Current.Resources,
            Path = new PropertyPath("GrayColor4")
        };
        button.SetBinding(Button.ForegroundProperty, foregroundBinding);

        var selectAllGeometry = (Geometry)FindResource("AddGeometry");
        IconElement.SetGeometry(button, selectAllGeometry);

        button.Click += AddAttribute;
        dynamicGrid.Children.Add(textBlock);
        dynamicGrid.Children.Add(button);

        AttributePanel.Children.Add(dynamicGrid);

        Border border = new Border
        {
            Height = 120,
            Background = Brushes.White,
            CornerRadius = new CornerRadius(8),
            Margin = new Thickness(5, 5, 5, 0)
        };

        ScrollViewer scrollViewer = new ScrollViewer();
        StackPanel stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        Control = stackPanel;
        scrollViewer.Content = Control;
        border.Child = scrollViewer;

        AttributePanel.Children.Add(border);

        if (defaultValue is bool b)
        {
            if (b)
                AddAttribute(stackPanel, true);
        }
        else if (defaultValue is string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
                AddAttribute(stackPanel, s);
        }
        else if (defaultValue is List<string> ls)
        {
            foreach (var VARIABLE in ls)
            {
                AddAttribute(stackPanel, VARIABLE);
            }
        }
        else if (defaultValue is List<int> li)
        {
            foreach (var VARIABLE in li)
            {
                AddAttribute(stackPanel, VARIABLE);
            }
        }
    }

    private void SwitchListList(object? defaultValue)
    {
        AttributePanel.Children.Clear();

        Grid dynamicGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 5, 10, 4)
        };

        TextBlock textBlock = new TextBlock
        {
            Margin = new Thickness(0, 0, 0, 0),
            Width = 55,
            Text = "属性值：",
            Foreground = (Brush)Application.Current.Resources["GrayColor1"],
            HorizontalAlignment = HorizontalAlignment.Left
        };

        Button button = new Button
        {
            Style = FindResource("textBoxButton") as Style,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 15, Padding = new Thickness(0),
            Height = 15
        };
        button.BindLocalization("Add", Button.ToolTipProperty);
        Binding foregroundBinding = new Binding
        {
            Source = Application.Current.Resources,
            Path = new PropertyPath("GrayColor4")
        };
        button.SetBinding(Button.ForegroundProperty, foregroundBinding);

        var selectAllGeometry = (Geometry)FindResource("AddGeometry");
        IconElement.SetGeometry(button, selectAllGeometry);

        button.Click += AddAttribute;
        dynamicGrid.Children.Add(textBlock);
        dynamicGrid.Children.Add(button);

        AttributePanel.Children.Add(dynamicGrid);

        Border border = new Border
        {
            Height = 120,
            Background = Brushes.White,
            CornerRadius = new CornerRadius(8),
            Margin = new Thickness(5, 5, 5, 0)
        };

        ScrollViewer scrollViewer = new ScrollViewer();

        StackPanel stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical
        };
        Control = stackPanel;
        scrollViewer.Content = Control;
        border.Child = scrollViewer;

        AttributePanel.Children.Add(border);
        if (defaultValue is List<List<int>> ls)
        {
            foreach (var VARIABLE in ls)
            {
                AddAttribute(stackPanel, string.Join(",", VARIABLE));
            }
        }
        else if (defaultValue is List<int> li)
        {
            foreach (var VARIABLE in li)
            {
                AddAttribute(stackPanel, VARIABLE);
            }
        }
    }

    private void SwitchByType(string selectedType, object? defaultValue)
    {
        if (selectedType != null)
        {
            switch (selectedType)
            {
                //List<string>
                case "next":
                case "timeout_next":
                case "runout_next":
                    SwitchAutoList(defaultValue);
                    break;
                case "expected":
                case "template":
                case "labels":
                case "focus_tip":
                case "focus_tip_color":
                    SwitchList(defaultValue);
                    break;
                //List<int>
                case "target_offset":
                case "begin_offset":
                case "end_offset":
                case "key":
                case "upper":
                case "lower":
                    SwitchList(defaultValue);
                    break;
                //List<List<int>>
                case "roi":
                    SwitchListList(defaultValue);
                    break;
                //bool
                case "is_sub":
                case "inverse":
                case "enabled":
                case "focus":
                case "only_rec":
                case "green_mask":
                case "connected":
                    SwitchBool(defaultValue);
                    break;
                //uint
                case "timeout":
                case "times_limit":
                case "pre_delay":
                case "post_delay":
                case "duration":
                    SwitchString(defaultValue);
                    break;
                //int
                case "index":
                case "method":
                case "count":
                    SwitchString(defaultValue);
                    break;
                //double
                case "ratio":
                case "threshold":
                    SwitchString(defaultValue);
                    break;
                //coombo
                case "recognition":
                case "action":
                case "order_by":
                case "detector":
                    SwitchCombo(selectedType, defaultValue);
                    break;
                //object
                case "target":
                case "begin":
                case "end":
                    SwitchAutoList(defaultValue);
                    break;
                //string
                default:
                    SwitchString(defaultValue);
                    break;
            }
        }
    }

    private void ReadValue(string selectedType)
    {
        if (selectedType != null)
        {
            switch (selectedType)
            {
                //List<string>
                case "next":
                case "timeout_next":
                case "runout_next":

                    if (Control is StackPanel p0)
                    {
                        var list = new List<string>();
                        foreach (var VARIABLE in p0.Children)
                        {
                            if (VARIABLE is SAutoCompleteTextBox sAutoCompleteTextBox)
                            {
                                if (!string.IsNullOrEmpty(sAutoCompleteTextBox.Text))
                                    list.Add(sAutoCompleteTextBox.Text);
                            }
                            else if (VARIABLE is TextBox textBox)
                            {
                                if (!string.IsNullOrEmpty(textBox.Text))
                                    list.Add(textBox.Text);
                            }
                        }

                        Attribute.Value = list;
                    }
                    break;
                case "expected":
                case "template":
                case "labels":
                case "focus_tip":
                case "focus_tip_color":
                    if (Control is StackPanel { Children: var children })
                    {
                        var list = children
                            .OfType<TextBox>()
                            .Where(textBox => !string.IsNullOrEmpty(textBox.Text))
                            .Select(textBox => textBox.Text)
                            .ToList();

                        Attribute.Value = list;
                    }


                    break;
                //List<int>
                case "target_offset":
                case "begin_offset":
                case "end_offset":
                case "key":
                    if (Control is StackPanel { Children: var child })
                    {
                        var list = child
                            .OfType<TextBox>()
                            .Select(textBox => int.TryParse(textBox.Text, out var i) ? i : (int?)null)
                            .Where(i => i.HasValue)
                            .Select(i => i ?? 0)
                            .ToList();

                        Attribute.Value = list;
                    }


                    break;
                //List<List<int>>
                case "upper":
                case "lower":
                case "roi":
                    if (Control is StackPanel { Children.Count: > 0 } p3)
                    {
                        if (p3.Children[0] is TextBox { Text: var text } && text.Contains(","))
                        {
                            try
                            {
                                var list = p3.Children
                                    .OfType<TextBox>()
                                    .Select(tb => tb.Text.Split(",").Select(int.Parse).ToList())
                                    .ToList();
                                Attribute.Value = list;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                LoggerService.LogError(e);
                                Growls.WarningGlobal("读取数字数组时出现错误！");
                            }
                        }
                        else
                        {
                            var list = p3.Children
                                .OfType<TextBox>()
                                .Select(tb => int.TryParse(tb.Text, out var i) ? i : (int?)null)
                                .Where(i => i.HasValue)
                                .Select(i => i ?? 0)
                                .ToList();
                            Attribute.Value = list;
                        }
                    }

                    break;
                //bool
                case "is_sub":
                case "inverse":
                case "enabled":
                case "focus":
                case "only_rec":
                case "green_mask":
                case "connected":
                    if (Control is ComboBox cb1)
                        Attribute.Value = cb1.SelectedValue;
                    break;
                //uint
                case "timeout":
                case "times_limit":
                case "pre_delay":
                case "post_delay":
                case "duration":
                    if (Control is TextBox t1)
                        Attribute.Value = uint.TryParse(t1.Text, out var ui) ? ui : 0;

                    break;
                //int
                case "index":
                case "method":
                case "count":
                    if (Control is TextBox t2)
                        Attribute.Value = int.TryParse(t2.Text, out var i) ? i : 0;

                    break;
                //double
                case "ratio":
                case "threshold":
                    if (Control is TextBox t3)
                        Attribute.Value = double.TryParse(t3.Text, out var d) ? d : 0;
                    break;
                //coombo
                case "recognition":
                case "action":
                case "order_by":
                case "detector":
                    if (Control is ComboBox cb2)
                        Attribute.Value = cb2.SelectedValue;
                    break;
                //object
                case "target":
                case "begin":
                case "end":
                    if (Control is StackPanel { Children: var children1 })
                    {
                        if (children1.Count == 1 && children1[0] is SAutoCompleteTextBox textBox)
                        {
                            if (bool.TryParse(textBox.Text, out var b) && b)
                            {
                                Attribute.Value = true;
                            }
                            else
                            {
                                Attribute.Value = textBox.Text;
                            }
                        }
                        else if (children1.Count > 0)
                        {
                            if (children1[0] is SAutoCompleteTextBox { Text: var text } && text.Contains(","))
                            {
                                try
                                {
                                    var list = children1
                                        .OfType<TextBox>()
                                        .Select(tb => tb.Text.Split(",").Select(int.Parse).ToList())
                                        .ToList();
                                    Attribute.Value = list;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    LoggerService.LogError(e);
                                    Growls.WarningGlobal("读取数字数组时出现错误！");
                                }
                            }
                            else
                            {
                                var list = children1
                                    .OfType<TextBox>()
                                    .Select(tb => int.TryParse(tb.Text, out var i) ? i : (int?)null)
                                    .Where(i => i.HasValue)
                                    .Select(i => i ?? 0) 
                                    .ToList();
                                Attribute.Value = list;
                            }
                        }
                    }


                    break;
                //string
                default:
                    if (Control is TextBox t4)
                        Attribute.Value = t4.Text;
                    break;
            }
        }
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedType = (string)typeComboBox.SelectedValue;
        // var selectedAttributeType = (string)attributeTypeComboBox.SelectedItem;
        SwitchByType(selectedType, null);
    }

    private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T t)
            {
                return t;
            }

            if (child != null)
            {
                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    private void AddAutoAttribute(object sender, RoutedEventArgs e)
    {
        AddAutoAttribute(sender);
    }

    private void AddAutoAttribute(object sender, object? content = null)
    {
        if (sender is Button button)
        {
            // 找到根 Grid
            var grid = (Grid)VisualTreeHelper.GetParent(button);
            var rootPanel = (StackPanel)VisualTreeHelper.GetParent(grid);

            // 找到 ScrollViewer
            var scrollViewer = FindVisualChild<ScrollViewer>(rootPanel);
            if (scrollViewer == null)
                return;
            // 找到 ScrollViewer 内部的 StackPanel
            var stackPanel = FindVisualChild<StackPanel>(scrollViewer);
            AddAutoAttribute(stackPanel, content);
        }
    }

    private void AddAutoAttribute(Panel? panel, object? content)
    {
        if (panel != null)
        {
            var contextMenu = new ContextMenu();
            var deleteItem = new MenuItem { Header = "删除" };
            deleteItem.Click += DeleteAttribute;
            contextMenu.Items.Add(deleteItem);
            var taskDialog = ParentDialog as EditTaskDialog;
            if (taskDialog == null)
                return;
            var newTextBox = new SAutoCompleteTextBox
            {
                Margin = new Thickness(5, 2, 5, 2), DisplayMemberPath = "Name",
                DataList = taskDialog.Data?.DataList, ItemsSource = taskDialog.Data?.DataList
            };
            if (content != null)
                newTextBox.Text = content.ToString();
            newTextBox.ContextMenu = contextMenu;

            panel.Children.Add(newTextBox);
        }
    }


    private void AddAttribute(Panel? panel, object? content)
    {
        if (panel != null)
        {
            var contextMenu = new ContextMenu();
            var deleteItem = new MenuItem { Header = "删除" };
            deleteItem.Click += DeleteAttribute;
            contextMenu.Items.Add(deleteItem);

            var newTextBox = new TextBox
            {
                Margin = new Thickness(5, 2, 5, 2)
            };
            if (content != null)
                newTextBox.Text = content.ToString();
            newTextBox.ContextMenu = contextMenu;
            panel.Children.Add(newTextBox);
        }
    }

    private void AddAttribute(object sender, object? content = null)
    {
        if (sender is Button button)
        {
            // 找到根 Grid
            var grid = (Grid)VisualTreeHelper.GetParent(button);
            var rootPanel = (StackPanel)VisualTreeHelper.GetParent(grid);

// 找到 ScrollViewer
            var scrollViewer = FindVisualChild<ScrollViewer>(rootPanel);
            if (scrollViewer is null)
                return;

// 找到 ScrollViewer 内部的 StackPanel
            var stackPanel = FindVisualChild<StackPanel>(scrollViewer);
            AddAttribute(stackPanel, content);
        }
    }

    private void AddAttribute(object sender, RoutedEventArgs e)
    {
        AddAttribute(sender);
    }

    private void DeleteAttribute(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuItem;
        var contextMenu = menuItem?.Parent as ContextMenu;

        if (contextMenu?.PlacementTarget is Control { Parent: Panel { Children: var children } } control)
            children.Remove(control);
    }
}