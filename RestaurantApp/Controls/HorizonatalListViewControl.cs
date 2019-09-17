using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.Controls
{
    public class HorizonatalListViewControl : Grid
    {

        private ICommand _innerSelectedCommand;

        private readonly ScrollView _scrollView;

        private readonly StackLayout _itemsStackLayout;



        public event EventHandler SelectedItemChanged;



        public StackOrientation ListOrientation { get; set; }



        public double Spacing { get; set; }


        public void GetChildWithIndex(int index)
        {
            var targetView = _itemsStackLayout.Children[index];
            _scrollView.ScrollToAsync(targetView, ScrollToPosition.Start, true);

        }

        public static readonly BindableProperty SelectedCommandProperty =

                    BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(HorizonatalListViewControl), null);



        public static readonly BindableProperty ItemsSourceProperty =

            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(HorizonatalListViewControl), default(IEnumerable<object>), BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);



        public static readonly BindableProperty SelectedItemProperty =

            BindableProperty.Create("SelectedItem", typeof(object), typeof(HorizonatalListViewControl), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);



        public static readonly BindableProperty ItemTemplateProperty =

            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(HorizonatalListViewControl), default(DataTemplate));

        public static readonly BindableProperty ScrollViewProperty =
             BindableProperty.Create("ScrollView", typeof(ScrollView), typeof(HorizonatalListViewControl), default(ScrollView));

        public static readonly BindableProperty ScrollBarVisibilityProperty =
            BindableProperty.Create("ScrollBarVisibility", typeof(ScrollBarVisibility), typeof(HorizonatalListViewControl), ScrollBarVisibility.Never);

        public ScrollBarVisibility scrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(ScrollBarVisibilityProperty); }
            set { SetValue(ScrollBarVisibilityProperty, value); }
        }
        public ScrollView ScrollView
        {
            get { return (ScrollView)GetValue(ScrollViewProperty); }
            set { SetValue(ScrollViewProperty, value); }
        }

        public ICommand SelectedCommand

        {

            get { return (ICommand)GetValue(SelectedCommandProperty); }

            set { SetValue(SelectedCommandProperty, value); }

        }



        public IEnumerable ItemsSource

        {

            get { return (IEnumerable)GetValue(ItemsSourceProperty); }

            set { SetValue(ItemsSourceProperty, value); }

        }



        public object SelectedItem

        {

            get { return (object)GetValue(SelectedItemProperty); }

            set { SetValue(SelectedItemProperty, value); }

        }



        public DataTemplate ItemTemplate

        {

            get { return (DataTemplate)GetValue(ItemTemplateProperty); }

            set { SetValue(ItemTemplateProperty, value); }

        }



        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)

        {

            var itemsLayout = (HorizonatalListViewControl)bindable;

            itemsLayout.SetItems();

        }



        public HorizonatalListViewControl()

        {

            _scrollView = new ScrollView();



            _itemsStackLayout = new StackLayout

            {

                BackgroundColor = BackgroundColor,

                Padding = Padding,

                Spacing = Spacing,

                HorizontalOptions = LayoutOptions.FillAndExpand

            };



            _scrollView.BackgroundColor = BackgroundColor;

            _scrollView.Content = _itemsStackLayout;

            Children.Add(_scrollView);

        }



        protected virtual void SetItems()

        {

            _itemsStackLayout.Children.Clear();

            _itemsStackLayout.Spacing = Spacing;



            _innerSelectedCommand = new Command<View>(view =>

            {

                SelectedItem = view.BindingContext;

                SelectedItem = null; // Allowing item second time selection

            });



            _itemsStackLayout.Orientation = ListOrientation;

            _scrollView.Orientation = ListOrientation == StackOrientation.Horizontal

                ? ScrollOrientation.Horizontal

                : ScrollOrientation.Vertical;



            if (ItemsSource == null)

            {

                return;

            }



            foreach (var item in ItemsSource)

            {
                _itemsStackLayout.Children.Add(GetItemView(item));

            }



            _itemsStackLayout.BackgroundColor = BackgroundColor;

            SelectedItem = null;

        }



        protected virtual View GetItemView(object item)

        {

            var content = ItemTemplate.CreateContent();

            var view = content as View;



            if (view == null)

            {

                return null;

            }



            view.BindingContext = item;



            var gesture = new TapGestureRecognizer

            {

                Command = _innerSelectedCommand,

                CommandParameter = view

            };



            AddGesture(view, gesture);



            return view;

        }



        private void AddGesture(View view, TapGestureRecognizer gesture)

        {

            view.GestureRecognizers.Add(gesture);



            var layout = view as Layout<View>;



            if (layout == null)

            {

                return;

            }



            foreach (var child in layout.Children)

            {

                AddGesture(child, gesture);

            }

        }



        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)

        {

            var itemsView = (HorizonatalListViewControl)bindable;

            if (newValue == oldValue && newValue != null)

            {

                return;

            }



            itemsView.SelectedItemChanged?.Invoke(itemsView, EventArgs.Empty);



            if (itemsView.SelectedCommand?.CanExecute(newValue) ?? false)

            {

                itemsView.SelectedCommand?.Execute(newValue);

            }

        }

    }
}
