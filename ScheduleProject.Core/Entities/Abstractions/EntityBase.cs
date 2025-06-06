﻿using CSharpFunctionalExtensions;

namespace ScheduleProject.Core.Entities.Abstractions;

public class EntityBase : Entity, IAuditable, IDeletable
{
	protected EntityBase(long id) : base(id) {	}
	protected EntityBase() : base() {	}

	public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
	public DateTime? ModifiedAt { get; protected set; }
	public bool IsDeleted { get; protected set; } = false;
	public DateTime? DeletionDate { get; protected set; } = null;

	public void SetDeleted(bool isDeleted)
	{
		IsDeleted = isDeleted;
		DeletionDate = IsDeleted ? DateTime.UtcNow : null;
	}

	public void MarkAsModified()
	{
		ModifiedAt = DateTime.UtcNow;
	}
}
