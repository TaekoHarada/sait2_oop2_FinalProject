
namespace FinalProject.Models
{
	public class JobTask
	{
		public string TaskId { get; set; }
		public string TaskName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Status { get; set; }
		public string ProjectId { get; set; }

	}
}
