using FinalProject.Models;
using FinalProject.Database;

namespace FinalProject.Services
{
	public class JobTaskService
	{
		private JobTaskDBAccessor jobTaskDBAccessor = new JobTaskDBAccessor();

		public JobTaskService()
		{
			jobTaskDBAccessor.CreateTable();
		}

		public List<JobTask> SearchAll()
		{
			List<JobTask> jobTasks = jobTaskDBAccessor.SelectAll();
			return jobTasks;
		}
		public List<JobTask> SearchByProject(string projectId)
		{
			List<JobTask> jobTasks = jobTaskDBAccessor.SelectByProject(projectId);
			return jobTasks;
		}
		public JobTask SearchById(string taskId)
		{
			JobTask jobTask = jobTaskDBAccessor.SelectById(taskId);
			return jobTask;
		}
		public async Task Add(JobTask jobTask)
		{
			if (jobTask.TaskName == null) { return; }

			 if (jobTask.TaskName.Trim() != string.Empty)
			{
				await jobTaskDBAccessor.Insert(jobTask);
			}
		}
		public async Task Edit(JobTask jobTask)
		{
			if (jobTask.TaskName == null) { return; }

			if (jobTask.TaskName.Trim() != string.Empty)
			{
				await jobTaskDBAccessor.Update(jobTask);
			}
		}
		public async Task Remove(JobTask jobTask)
		{
			if (jobTask != null)
			{
				await jobTaskDBAccessor.Delete(jobTask);
			}

		}

	}
}
