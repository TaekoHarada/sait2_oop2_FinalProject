using Dapper;
using FinalProject.Models;
using MySqlConnector;

namespace FinalProject.Database
{
	public class JobTaskMemberLinkDBAccessor : DBAccessor
	{
		public override void CreateTable()
		{
			connection.Open();

			var sql = @"CREATE TABLE IF NOT EXISTS JobTaskMemberLink (
                TaskId VARCHAR(36) NOT NULL,
                MemberId VARCHAR(36) NOT NULL,
  					PRIMARY KEY (TaskId, MemberId),
				    CONSTRAINT JTML_TaskId_FK FOREIGN KEY (TaskId) REFERENCES JobTask(TaskId),
				    CONSTRAINT JTML_MemberId_FK FOREIGN KEY (MemberId) REFERENCES Member(MemberId)
            )";

			connection.Execute(sql);

			connection.Close();

		}

		public Task Insert(JobTaskMemberLink jobTaskMemberLink)
		{
			try
			{
				connection.Open();

				string sql = "INSERT INTO JobTaskMemberLink (TaskId, MemberId) VALUES (@TaskId, @MemberId)";

				connection.Execute(sql, jobTaskMemberLink);
			}
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}
			return Task.CompletedTask;
		}



		public Task Delete(JobTaskMemberLink jobTaskMemberLink)
		{
			try
			{
				connection.Open();

				string sql = "DELETE FROM JobTaskMemberLink WHERE TaskId = @TaskId AND  MemberId = @MemberId";

				connection.Execute(sql, jobTaskMemberLink);
			}
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}
			return Task.CompletedTask;
		}




	}
}
