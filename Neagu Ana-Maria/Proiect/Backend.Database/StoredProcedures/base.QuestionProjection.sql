CREATE PROCEDURE [base].[QuestionProjection] @QuestionId INT
AS
BEGIN
    SELECT q.PostId,
           q.PostTypeId,
           q.ParentPostId,
           q.Title,
           q.PostText,
           q.PostedBy,
           q.AcceptedAnswer,
           q.DateCreated,
           q.Closed,
           q.ClosedBy,
           q.ClosedDate,
           q.LastUpdatedBy,
           q.RowGuid,
           q.SysStartTime,
           q.SysEndTime,
           q.RowVersion,

           a.PostId,
           a.PostTypeId,
           a.ParentPostId,
           a.Title,
           a.PostText,
           a.PostedBy,
           a.AcceptedAnswer,
           a.DateCreated,
           a.Closed,
           a.ClosedBy,
           a.ClosedDate,
           a.LastUpdatedBy,
           a.RowGuid,
           a.SysStartTime,
           a.SysEndTime,
           a.RowVersion,

           v.QuestionId,
           v.VoteTypeId,
           v.UserId,
           v.DateCreated,
           v.VoteValue

    FROM [base].Post q
        LEFT JOIN [base].Post a
            ON a.TenantId = q.TenantId
               AND a.ParentPostId = q.PostId
        LEFT JOIN [base].Vote v
            ON v.TenantId = q.TenantId
               AND v.QuestionId = q.PostId
    WHERE q.PostId = @QuestionId;
END;
