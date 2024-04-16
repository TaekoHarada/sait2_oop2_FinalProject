using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;
using System;
using BlazorBootstrap;

namespace FinalProject.Components.Pages
{
	public partial class JobTaskList : ComponentBase
	{
		[Parameter]
		public string ProjectId { get; set; }
		[Inject] NavigationManager NavigationManager { get; set; }

		private Project project;
		private List<JobTask> jobTasks;
		private JobTask newJobTask = new JobTask();
		private DateOnly StartDateDateonly;
		private DateOnly EndDateDateonly;

		private JobTaskService jobTaskService = new JobTaskService();
		private ProjectService projectService = new ProjectService();

		// for switching sort
		private bool IsAscStart = false;
		private bool IsAscEnd = false;
		private bool IsAscStatus = false;
		protected override void OnInitialized()
		{
			newJobTask.StartDate = DateTime.Now;
			newJobTask.EndDate = DateTime.Now;
			// Convert to DateOnly for view page
			StartDateDateonly = DateOnly.FromDateTime(newJobTask.StartDate);
			EndDateDateonly = DateOnly.FromDateTime(newJobTask.EndDate);

			newJobTask.ProjectId = ProjectId;
			newJobTask.Status = 0;

			project = projectService.SearchById(ProjectId);
			jobTasks = jobTaskService.SearchByProject(ProjectId);

			// Sort
			SortStartDate();
		}
		private void SortStartDate()
		{
			if (IsAscStart)
			{
				jobTasks.Sort((x, y) => y.StartDate.CompareTo(x.StartDate));
			}
			else
			{
				jobTasks.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
			}

			IsAscStart = !IsAscStart;
		}
		private void SortEndDate()
		{
			if (IsAscEnd)
			{
				jobTasks.Sort((x, y) => y.EndDate.CompareTo(x.EndDate));
			}
			else
			{
				jobTasks.Sort((x, y) => x.EndDate.CompareTo(y.EndDate));
			}

			IsAscEnd = !IsAscEnd;
		}

		private void SortStatus()
		{
			if (IsAscStatus)
			{
				jobTasks.Sort((x, y) => y.Status.CompareTo(x.Status));
			}
			else
			{
				jobTasks.Sort((x, y) => x.Status.CompareTo(y.Status));
			}

			IsAscStatus = !IsAscStatus;
		}

		private async Task AddHandler()
		{
			newJobTask.TaskId = Guid.NewGuid().ToString();
			// Convert to DateTime (Dapper support)
			newJobTask.StartDate = new DateTime(StartDateDateonly.Year, StartDateDateonly.Month, StartDateDateonly.Day);
			newJobTask.EndDate = new DateTime(EndDateDateonly.Year, EndDateDateonly.Month, EndDateDateonly.Day);

			await jobTaskService.Add(newJobTask);

			OnInitialized();
		}

		private void MembersHandler(JobTask jobTask)
		{
			NavigationManager.NavigateTo($"/JobTaskMember/{ProjectId}/{jobTask.TaskId}");
		}
		private void EditHandler(JobTask jobTask)
		{
			NavigationManager.NavigateTo($"/EditJobTask/{ProjectId}/{jobTask.TaskId}");
		}

		private async Task DeleteHandler(JobTask jobTask)
		{
			await jobTaskService.Remove(jobTask);
			OnInitialized();
		}
		private void ReturnPageHandler()
		{
			NavigationManager.NavigateTo("/");
		}

	}
}
