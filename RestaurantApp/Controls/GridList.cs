using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace RestaurantApp.Controls
{
    public class GridList : StackLayout
    {
        public static readonly BindableProperty ItemTemplateProperty =
               BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(GridList), default(DataTemplate));

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(GridList), null, BindingMode.OneWay, propertyChanged: ItemsChanged);
        public static readonly BindableProperty ItemsOnRowProperty =
            BindableProperty.Create(nameof(ItemsOnRow), typeof(int), typeof(GridList), 1);
        public GridList()
        {
            Spacing = 0;
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public int ItemsOnRow
        {
            get => (int)GetValue(ItemsOnRowProperty);
            set => SetValue(ItemsOnRowProperty, value);
        }
        protected virtual View ViewFor(object item)
        {
            View view = null;

            if (ItemTemplate != null)
            {
                var content = ItemTemplate.CreateContent();

                view = content is View ? content as View : ((ViewCell)content).View;

                view.BindingContext = item;
            }

            return view;
        }

        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is GridList control)) return;

            control.Children.Clear();

            control.Orientation = StackOrientation.Vertical;

            StackLayout stack = new StackLayout() { Orientation = StackOrientation.Horizontal };
            //stack.Orientation = StackOrientation.Horizontal;
            var items = (ICollection)newValue;
            if (items == null) return;
            //var list = items.Cast<IList>();
            var listCount = items.Count;
            object[] list = new object[listCount];
            items.CopyTo(list, 0);

            for (int i = 0; i < listCount; i++)
            {
                if (stack.Children.Count == control.ItemsOnRow)
                {
                    StackLayout newstack = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    newstack = stack;
                    control.Children.Add(newstack);
                    var count = newstack.Children.Count;
                    stack = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    stack.Children.Add(control.ViewFor(list.ElementAt(i)));
                    continue;
                }
                stack.Children.Add(control.ViewFor(list.ElementAt(i)));
            }
            control.Children.Add(stack);
        }
    }
}
