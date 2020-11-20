CREATE PROCEDURE [base].[QuestionsProjection] @TenantId INT
AS
BEGIN
    SELECT TenantId,
           PostId,
           PostTypeId,
           ParentPostId,
           Title,
           PostText,
           PostedBy,
           AcceptedAnswer,
           DateCreated,
           Closed,
           ClosedBy,
           ClosedDate,
           LastUpdatedBy,
           RowGuid,
           SysStartTime,
           SysEndTime,
           RowVersion
    FROM [base].Post
    WHERE TenantId = @TenantId
		AND PostTypeId = 1
END;