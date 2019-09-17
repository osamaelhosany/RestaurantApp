using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using RestaurantApp.Controls;
using RestaurantApp.Droid.Renderer;
using RestaurantApp.Enums;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace RestaurantApp.Droid.Renderer
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;

        public ImageEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 25;
            Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Element is ImageEntry imageEntry)
            {
                if (e.PropertyName == "Image")
                {
                    element = (ImageEntry)this.Element;


                    var editText = this.Control;
                    if (!string.IsNullOrEmpty(element.Image))
                    {
                        switch (element.ImageAlignment)
                        {
                            case ImageAlignment.Left:
                                editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                                break;
                            case ImageAlignment.Right:
                                editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                                break;
                        }
                    }
                    else if (element.Image == "")
                    {
                        switch (element.ImageAlignment)
                        {
                            case ImageAlignment.Left:
                                editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
                                break;
                            case ImageAlignment.Right:
                                editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, null);
                                break;
                        }
                    }
                    editText.CompoundDrawablePadding = 25;
                    Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }

            }

        }

    }

}