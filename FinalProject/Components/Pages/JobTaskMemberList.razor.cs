using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;

namespace FinalProject.Components.Pages
{
	public partial class JobTaskMemberList : ComponentBase
	{
		private Project project;
		private JobTask jobTask;

		// for searching
		private string SearchMemberName { get; set; } = "";
		private List<Member> searchMembers = new List<Member>();
		private List<Member> newmembers = new List<Member>();

		// selected MemberId to add as a new member
		private string SelectedMemberId { get; set; } = "";

		// Members for this task
		private List<Member> taskMembers = new List<Member>();

		// To show message
		private bool isSaved = false;

		[Parameter]
		public string ProjectId { get; set; }

		[Parameter]
		public string TaskId { get; set; }

		[Inject] NavigationManager NavigationManager { get; set; }

		private ProjectService projectService = new ProjectService();
		private JobTaskService jobTaskService = new JobTaskService();
		private MemberService memberService = new MemberService();

		private JobTaskMemberLinkService jobTaskMemberLinkService = new JobTaskMemberLinkService();


		protected override void OnInitialized()
		{
			project = projectService.SearchById(ProjectId);
			jobTask = jobTaskService.SearchById(TaskId);
			taskMembers = memberService.SearchByTask(TaskId);
			searchMembers = memberService.SearchByName(SearchMemberName);
			newmembers = new List<Member>(searchMembers);

			// newmembers = searchMembers - taskMember			
			foreach (Member searchMember in searchMembers)
			{
				if (taskMembers.Any(taskMember => searchMember.MemberId == taskMember.MemberId))
				{
					newmembers.Remove(searchMember);
				}
			}


			//SelectedMemberId = newmembers.First().MemberId;
		}

		private void SearchMemberHandler() 
		{
			searchMembers = memberService.SearchByName(SearchMemberName).Except(taskMembers).ToList();
			SelectedMemberId = searchMembers.First().MemberId;
		}

		private void AddMemberToTaskHandler()
		{
			//memberId, taskId to insert JobTaskMemberLink DB
			jobTaskMemberLinkService.Add(new JobTaskMemberLink(TaskId, SelectedMemberId));

			OnInitialized();
			SearchMemberHandler();
		}

		private void DeleteHandler(Member member)
		{
			jobTaskMemberLinkService.Remove(new JobTaskMemberLink(TaskId, member.MemberId));
			OnInitialized();

		}

		private void ReturnPageHandler()
		{
			NavigationManager.NavigateTo($"/JobTaskList/{ProjectId}");
		}


	}
}
