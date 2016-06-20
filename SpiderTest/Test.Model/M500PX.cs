using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Model
{
    public class M500PX
    {
        public int current_page { get; set; }

        public int total_pages { get; set; }

        public int total_items { get; set; }

        public string feature { get; set; }

        public List<PhotosEntity> photos { get; set; }
    }

    public class PhotosEntity
    {
        public int id { get; set; }

        public List<ImagesEntity> images { get; set; }
    }

    public class ImagesEntity
    {

        public int size { get; set; }

        public string url { get; set; }

        public string https_url { get; set; }

        public string format { get; set; }
    }
}
