using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Helpers
{
	public class FileModel
	{
		public string FileName { get; set; } = "";
		public string RelativePath { get; set; } = "";
		public string Url { get; set; } = "";
		public long Size { get; set; }
		public bool IsImage { get; set; }
		public bool IsVideo { get; set; }
		public string UploadedBy { get; set; } = "";
		public string Tag { get; set; } = "";

	}
}
