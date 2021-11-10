using System;
using System.Collections.Generic;
using System.Text;

namespace LG_Exam_ImageConverterPipeline.model
{
    public class config_DataModel
    {
        public int[] target_sizes { get; set; }
        public int bit_depth { get; set; }
        public string format { get; set; }
    }

    public class config_data
    {
        public config_DataModel config { get; set; }
    }
}
