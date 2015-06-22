using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpProjekt
{
    //core information about a picture
    public class DAImage
    {
        //deviation ID
        public string d_ID;
        public string title;
        public string category;
        public string author;
        //weburl of the image
        public string img_url;
        //filetype with the . (.jpg, .bmp, .gif, ...)
        public string filetype;
        public int height;
        public int width;
        public string thumbnail_url;
        public string thumbnail_path;
        public string image_path;

        //This constructor is needed to make serialization with the JsonConverter easier
        //The JsonConverter calls the constructor of the object it is deserialising, and fills blank parametes with null
        //this lead to an NullReferenceException because of JsonReceiver.Content.src
        public DAImage()
        {

        }
        public DAImage(
            string _d_ID,
            string _title,
            string _category,
            string _author,
            JsonReceiver.Content content,
            string _thumbnail_url)
        {
            d_ID = _d_ID;
            title = _title;
            category = _category;
            author = _author;
            //this line caused the above mention NullReferenceException
            img_url = content.src;
            for (int i = img_url.LastIndexOf('.'); i < img_url.Length; i++)
            {
                filetype += img_url[i];
            }
            height = content.height;
            width = content.width;
            thumbnail_url = _thumbnail_url;
            thumbnail_path = "./thumbnails/thumb_" + d_ID + filetype;
            image_path = "./images/" + d_ID + filetype;
        }
    }
}
