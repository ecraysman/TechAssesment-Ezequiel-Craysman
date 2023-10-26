using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using DB.Models;

namespace API.Controllers
{
	public partial class AffiliatesController : ControllerBase
	{
		// PUT: api/Affiliates/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutAffiliates(Guid id, Affiliates affiliates)
		{
			if (id != affiliates.Id)//You can't send wrong id nor change it
			{
				return BadRequest();
			}

			var oldAffiliates = await _context.Affiliates.FindAsync(id);

			if (oldAffiliates == null)
			{
				return NotFound();
			}

			if (oldAffiliates.IsDeleted) return NotFound(); //Once you get deleted, you can't update it either

			//Before saving, validate we only have received the "Updateable info"
			//TODO: We should consider idempotency here, and only change fields that haven't been changed.

			bool IsValid = false;

			#region Validations

			//Name
			if (string.IsNullOrEmpty(oldAffiliates.Name))
			{
				IsValid = false;
				if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
				ListOfMessages += "Affiliate name is missing.";
			}
			//Email
			if (string.IsNullOrEmpty(oldAffiliates.Email))
			{
				IsValid = false;
				if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
				ListOfMessages += "Affiliate mail is missing.";
			}
			else
			{
				if (!Strings.IsValidEmail(oldAffiliates.Email))
				{
					IsValid = false;
					if (!string.IsNullOrEmpty(ListOfMessages)) ListOfMessages += System.Environment.NewLine;
					ListOfMessages += "Affiliate mail is invalid.";
				}
			}

			

			//Nothing else can be changed

			#endregion Validations

			if (IsValid)
			{
				_context.Entry(oldAffiliates).State = EntityState.Modified;


				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AffiliatesExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return NoContent();
			}
			else
			{
				return Problem(ListOfMessages);
			}
		}
	}
}
