using FinalProject.Models;
using FinalProject.Database;

namespace FinalProject.Services
{
	public class MemberService
	{
		private MemberDBAccessor memberDBAccessor = new MemberDBAccessor();

		public MemberService()
		{
			memberDBAccessor.CreateTable();
		}

		public List<Member> SearchAll()
		{
			List<Member> members = memberDBAccessor.SelectAll();
			return members;
		}

		public Member SearchById(string memberId)
		{
			Member member = memberDBAccessor.SelectById(memberId);
			return member;
		}

		public List<Member> SearchByName(string memberName)
		{
			List<Member> members = memberDBAccessor.SelectByName(memberName);
			return members;
		}

		public List<Member> SearchByTask(string taskId)
		{
			List<Member> members = memberDBAccessor.SelectByTask(taskId);
			return members;
		}
		public async Task Add(Member member)
		{
			if (member.MemberId.Trim() != string.Empty && member.MemberName.Trim() != string.Empty)
			{
				await memberDBAccessor.Insert(member);
			}
		}
		public async Task Edit(Member member)
		{
			if (member.MemberName.Trim() != string.Empty)
			{
				await memberDBAccessor.Update(member);
			}
		}
		public async Task Remove(Member member)
		{
			if (member != null)
			{
				await memberDBAccessor.Delete(member);
			}

		}

	}
}
