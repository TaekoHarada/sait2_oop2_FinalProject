using FinalProject.Models;
using FinalProject.Database;

namespace FinalProject.Services
{
	public class ProjectService
	{
		private ProjectDBAccessor projectDbAccessor = new ProjectDBAccessor();

		public ProjectService()
		{
			projectDbAccessor.CreateTable();
		}

		public List<Project> SearchAll()
		{
			List<Project> projects = projectDbAccessor.SelectAll();
			return projects;
		}
		public Project SearchById(string projectId)
		{
			Project project = projectDbAccessor.SelectById(projectId);
			return project;
		}
		public async Task Add(Project project)
		{
			if (project.ProjectName.Trim() != string.Empty)
			{
				await projectDbAccessor.Insert(project);
			}
		}
		public async Task Edit(Project project)
		{
			if (project.ProjectName.Trim() != string.Empty)
			{
				await projectDbAccessor.Update(project);
			}
		}
		public async Task Remove(Project project)
		{
			if (project != null)
			{
				await projectDbAccessor.Delete(project);
			}

		}
	}
}
