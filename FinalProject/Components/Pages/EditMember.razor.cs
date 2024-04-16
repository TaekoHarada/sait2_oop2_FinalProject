using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;

namespace FinalProject.Components.Pages
{
	public partial class EditMember : ComponentBase
	{
		private Member member;

		// To show message
		private bool isSaved = false;

		[Parameter]
		public string MemberId { get; set; }

		[Inject] NavigationManager NavigationManager { get; set; }

		private MemberService memberService = new MemberService();

		protected override void OnInitialized()
		{
			member = memberService.SearchById(MemberId);
		}

		private async Task UpdateHandler()
		{
			if (member != null)
			{
				await memberService.Edit(member);
				isSaved = true;
			}
		}

		private void ReturnPageHandler()
		{
			NavigationManager.NavigateTo("/MemberList");
		}
	}
}
