
namespace FinalProject.Models
{
	public class JobTaskMemberLink
	{
		public string TaskId { get; set; }
		public string MemberId { get; set; } 

		public JobTaskMemberLink(string taskId, string memberId)
		{
			TaskId = taskId;
			MemberId = memberId;
		}
	}
}
