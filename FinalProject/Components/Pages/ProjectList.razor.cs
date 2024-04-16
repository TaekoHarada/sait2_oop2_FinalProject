using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;


namespace FinalProject.Components.Pages
{
	public partial class ProjectList : ComponentBase
	{

		[Inject] NavigationManager NavigationManager { get; set; }


		private List<Project> projects;
		private Project newProject = new Project();

		private ProjectService projectService = new ProjectService();

		public ProjectList() { }

		protected override void OnInitialized()
		{
			projects = projectService.SearchAll();
		}

		private async Task AddHandler()
		{
			// Generate ProjectId
			newProject.ProjectId = Guid.NewGuid().ToString();

			await projectService.Add(newProject);
			OnInitialized();
		}
		private void EditHandler(Project project)
		{
			NavigationManager.NavigateTo($"/ProjectList/{project.ProjectId}");
		}

		private async Task DeleteHandler(Project project)
		{
			await projectService.Remove(project);
			OnInitialized();
		}
		private void GoToTaskListHandler(Project project)
		{
			NavigationManager.NavigateTo($"/JobTaskList/{project.ProjectId}");

		}

	}
}
