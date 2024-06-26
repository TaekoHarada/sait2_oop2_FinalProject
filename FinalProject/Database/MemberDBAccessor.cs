﻿using MySqlConnector;
using Dapper;
using FinalProject.Models;
using System.Net;
using System.Xml.Linq;

namespace FinalProject.Database
{
	public class MemberDBAccessor : DBAccessor
	{
		public override void CreateTable()
		{
			connection.Open();

			var sql = @"CREATE TABLE IF NOT EXISTS member (
                MemberId VARCHAR(36) PRIMARY KEY,
                MemberName VARCHAR(255) NOT NULL,
                Email VARCHAR(255),
                Phone VARCHAR(255) 
            )";

			connection.Execute(sql);

			connection.Close();

		}

		public Task Insert(Member member)
		{
			try
			{
				connection.Open();

				string sql = "INSERT INTO member (MemberId, MemberName, Email, Phone) VALUES (@MemberId, @MemberName, @Email, @Phone)";

				connection.Execute(sql, member);
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

		public List<Member> SelectAll()
		{
			List<Member> members = new List<Member>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM Member";

				members = connection.Query<Member>(sql).ToList();
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

			return members;
		}

		public List<Member> SelectByName(string memberName)
		{
			List<Member> members = new List<Member>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM Member where MemberName LIKE @MemberName";

				members = connection.Query<Member>(sql, new { MemberName = memberName + "%" }).ToList();
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

			return members;
		}

		public List<Member> SelectByTask(string taskId)
		{
			List<Member> members = new List<Member>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM Member INNER JOIN JobTaskMemberLink ON Member.MemberId=JobTaskMemberLink.MemberId where JobTaskMemberLink.TaskId=@TaskId";

				members = connection.Query<Member>(sql, new { TaskId = taskId }).ToList();
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

			return members;
		}

		public Member SelectById(string memberId)
		{
			Member member = new Member();
			try
			{
				connection.Open();

				string sql = $"SELECT * FROM Member WHERE memberId='{memberId}'";

				member = connection.QuerySingle<Member>(sql);
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

			return member;
		}

		public Task Update(Member member)
		{
			try
			{
				connection.Open();

				string sql = "UPDATE Member SET MemberId=@MemberId, MemberName=@MemberName, Email=@Email, Phone=@Phone WHERE MemberId=@MemberId";

				connection.Execute(sql, member);
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
		public async Task Delete(Member member)
		{
			await connection.OpenAsync();
			using (var transaction = await connection.BeginTransactionAsync())
			{
				try
				{
					// Delete from JobTaskMemberLink table
					string linkSql = "DELETE FROM JobTaskMemberLink WHERE MemberId = @MemberId";
					await connection.ExecuteAsync(linkSql, member, transaction: transaction);

					// Delete from Member table
					string memberSql = "DELETE FROM Member WHERE MemberId = @MemberId";
					await connection.ExecuteAsync(memberSql, member, transaction: transaction);

					await transaction.CommitAsync();
				}
				catch (MySqlException ex)
				{
					Console.WriteLine("MySQL Error:" + ex.Message);
					transaction.Rollback();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error:" + ex.Message);
					transaction.Rollback();
				}
				finally
				{
					connection.Close();
				}
			}
			//return Task.CompletedTask;
		}
	}
}
