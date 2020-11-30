using Access.Primitives.IO;
using StackUnderflow.Domain.Schema.Questions.CreateAnswerOp;
using System;
using System.Collections.Generic;
using System.Text;
using static StackUnderflow.Domain.Schema.Questions.CreateAnswerOp.CreateReplyResult;
using static PortExt;
using StackUnderflow.Domain.Schema.Questions.CheckLanguageOp;
using static StackUnderflow.Domain.Schema.Questions.CheckLanguageOp.CheckLanguageResult;
using static StackUnderflow.Domain.Schema.Questions.SendQuestionOwnerAcknowledgementOp.SendQuestionOwnerAcknowledgementResult;
using StackUnderflow.Domain.Schema.Questions.SendQuestionOwnerAcknowledgementOp;
using static StackUnderflow.Domain.Schema.Questions.SendReplyAuthorAcknowledgementOp.SendReplyAuthorAcknowledgementResult;
using StackUnderflow.Domain.Schema.Questions.SendReplyAuthorAcknowledgementOp;
using static StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp.CreateQuestionsResult;
using StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp;

namespace StackUnderflow.Domain.Core.Contexts.Questions
{
    public static class QuestionsContext
    {
        public static Port<ICreateQuestionsResult> CreateQuestion(CreateQuestionsCmd createQuestionsCmd) =>
            NewPort<CreateQuestionsCmd, ICreateQuestionsResult>(createQuestionsCmd);
        public static Port<ICreateReplyResult> CreateReply(CreateReplyCmd createReplyCmd) =>
           NewPort<CreateReplyCmd, ICreateReplyResult>(createReplyCmd);

        public static Port<ICheckLanguageResult> CheckLanguage(CheckLanguageCmd checkLanguageCmd) =>
            NewPort<CheckLanguageCmd, ICheckLanguageResult>(checkLanguageCmd);

        public static Port<ISendQuestionOwnerAcknowledgementResult> SendQuestionOwnerAcknowledgement(SendQuestionOwnerAcknowledgementCmd cmd) =>
            NewPort<SendQuestionOwnerAcknowledgementCmd, ISendQuestionOwnerAcknowledgementResult>(cmd);

        public static Port<ISendReplyAuthorAcknowledgementResult> SendReplyAuthorAcknowledgement(SendReplyAuthorAcknowledgementCmd cmd) =>
           NewPort<SendReplyAuthorAcknowledgementCmd, ISendReplyAuthorAcknowledgementResult>(cmd);
    }
}
