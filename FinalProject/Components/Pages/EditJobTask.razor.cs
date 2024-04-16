using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;

namespace FinalProject.Components.Pages
{
	public partial class EditJobTask : ComponentBase
	{

		private JobTask jobTask;

		// To show message
		private bool isSaved = false;

		[Parameter]
		public string ProjectId { get; set; }

		[Parameter]
		public string TaskId { get; set; }

		[Inject] NavigationManager NavigationManager { get; set; }

		private JobTaskService jobTaskService = new JobTaskService();

		private DateOnly StartDateDateonly;
		private DateOnly EndDateDateonly;


		protected override void OnInitialized()
		{
			jobTask = jobTaskService.SearchById(TaskId);

			// Convert for view page
			StartDateDateonly = DateOnly.FromDateTime(jobTask.StartDate);
			EndDateDateonly = DateOnly.FromDateTime(jobTask.EndDate);
		}

		private async Task UpdateHandler()
		{
			if (jobTask != null)
			{
				// Convert to DateTime (Dapper support)
				jobTask.StartDate = new DateTime(StartDateDateonly.Year, StartDateDateonly.Month, StartDateDateonly.Day);
				jobTask.EndDate = new DateTime(EndDateDateonly.Year, EndDateDateonly.Month, EndDateDateonly.Day);

				await jobTaskService.Edit(jobTask);
				isSaved = true;
			}
		}

		private void ReturnPageHandler()
		{
			NavigationManager.NavigateTo($"/JobTaskList/{ProjectId}");
		}

	}
}
