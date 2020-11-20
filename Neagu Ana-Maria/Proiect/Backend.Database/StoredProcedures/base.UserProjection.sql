CREATE PROCEDURE [base].[UserProjection] @UserId UNIQUEIDENTIFIER
AS
BEGIN
        SELECT p.PostId AS QuestionId,
           p.Title,
           p.AcceptedAnswer,
           p.DateCreated,
           p.SysStartTime AS LastUpdateText,
           ISNULL(v.Votes, 0) Votes,
           ISNULL(a.Answers, 0) Answers,
           ISNULL(pv.[Views], 0) [Views],
		   'c#, sql' AS Tags
    FROM [BASE].Post p
        LEFT JOIN
        (
            SELECT TenantId,
                   QuestionId,
                   SUM(   CASE
                              WHEN VoteTypeId = 1 THEN
                                  1
                              ELSE
                                  -1
                          END
                      ) AS Votes
            FROM BASE.Vote
            GROUP BY TenantId,
                     QuestionId
        ) v
            ON v.TenantId = p.TenantId
               AND v.QuestionId = p.PostId
        LEFT JOIN
        (
            SELECT TenantId,
                   ParentPostId,
                   COUNT(PostId) Answers
            FROM BASE.Post
            WHERE ParentPostId IS NOT NULL
            GROUP BY TenantId,
                     ParentPostId
        ) a
            ON a.TenantId = p.TenantId
               AND a.ParentPostId = p.PostId
        LEFT JOIN
        (
            SELECT TenantId,
                   PostId,
                   COUNT(DISTINCT UserId) AS [Views]
            FROM BASE.PostView
            GROUP BY TenantId,
                     PostId
        ) pv
            ON pv.TenantId = p.TenantId
               AND pv.PostId = p.PostId
        INNER JOIN BASE.TenantUser tu
            ON tu.TenantId = p.TenantId
    WHERE tu.UserId = @UserId
          AND p.PostTypeId = 1;
END;