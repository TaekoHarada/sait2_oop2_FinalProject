using Microsoft.AspNetCore.Components;
using FinalProject.Models;
using FinalProject.Services;

namespace FinalProject.Components.Pages
{
	public partial class MemberList : ComponentBase
	{
		[Inject] NavigationManager NavigationManager { get; set; }


		private List<Member> members;
		private Member newMember=new Member();

		private MemberService memberService = new MemberService();

		protected override void OnInitialized()
		{
			members = memberService.SearchAll();
		}

		private async Task AddHandler()
		{
			newMember.MemberId = Guid.NewGuid().ToString();

			await memberService.Add(newMember);
			OnInitialized();
		}

		private void EditHandler(Member member)
		{
			NavigationManager.NavigateTo($"/EditMember/{member.MemberId}");
		}

		private async Task DeleteHandler(Member member)
		{
			await memberService.Remove(member);
			OnInitialized();
		}


	}
}
