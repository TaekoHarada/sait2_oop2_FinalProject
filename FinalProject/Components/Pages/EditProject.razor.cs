using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;

namespace FinalProject.Components.Pages
{
	public partial class EditProject : ComponentBase
	{
		[Parameter]
		public string ProjectId { get; set; }

		[Inject] NavigationManager NavigationManager { get; set; }

		private Project project;

		// To show message
		private bool isSaved = false;

		private ProjectService projectService = new ProjectService();

		protected override void OnInitialized()
		{
			project = projectService.SearchById(ProjectId);
		}

		private async Task UpdateHandler()
		{
			if (project != null)
			{
				await projectService.Edit(project);
				isSaved = true;
			}
		}

		private void ReturnPageHandler()
		{
			NavigationManager.NavigateTo("/");
		}



	}
}
