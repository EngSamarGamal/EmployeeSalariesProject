namespace App.Common.Dtos.PaginaionModel
{

	public class PagainationModel<T>
	{
		public T Data { get; set; }
		public int TotalCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
