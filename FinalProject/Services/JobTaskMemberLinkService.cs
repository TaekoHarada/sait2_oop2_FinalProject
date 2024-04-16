using FinalProject.Models;
using FinalProject.Database;

namespace FinalProject.Services
{
	public class JobTaskMemberLinkService
	{
		private JobTaskMemberLinkDBAccessor jobTaskMemberLinkDBAccessor = new JobTaskMemberLinkDBAccessor();

		public JobTaskMemberLinkService()
		{
			jobTaskMemberLinkDBAccessor.CreateTable();
		}

		public async Task Add(JobTaskMemberLink jobTaskMemberLink)
		{
				await jobTaskMemberLinkDBAccessor.Insert(jobTaskMemberLink);
		}
		public async Task Remove(JobTaskMemberLink jobTaskMemberLink)
		{
				await jobTaskMemberLinkDBAccessor.Delete(jobTaskMemberLink);
		}

	}
}
