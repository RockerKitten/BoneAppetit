using JetBrains.Annotations;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Boneappetit
{
  /// <summary>
  /// Chef Hat Status Effect.
  /// </summary>
  [UsedImplicitly]
  // ReSharper disable once InconsistentNaming
  public class SE_CheffHat : SE_Stats
  {
    /// <summary>
    /// ctor
    /// </summary>
    public SE_CheffHat()
    {
      name = "rkCookingSkillStatusEffect";
      m_name = "Cooking Bonus";
      m_raiseSkill = BoneAppetit.Instance.rkCookingSkill;
      m_raiseSkillModifier = BoneAppetit.Instance.HatXpGain.Value;
      m_startMessageType = MessageHud.MessageType.Center;
      m_stopMessageType = MessageHud.MessageType.Center;
      SetMessages();
    }

    /// <summary>
    /// Randomly sets the status effect start and stop message
    /// </summary>
    private void SetMessages()
    {
      if (!BoneAppetit.Instance.HatSEMessage.Value)
      {
        if (string.IsNullOrEmpty(m_startMessage) && string.IsNullOrEmpty(m_stopMessage)) return;
        {
          m_startMessage = string.Empty;
          m_stopMessage = string.Empty;
          return;
        }
      }

      var current = m_startMessage;
      do
      {
        m_startMessage = RandomJuliaChildPhrase.GetRandomPhrase();
      } while (m_startMessage.Equals(m_stopMessage) || m_startMessage.Equals(current));

      current = m_stopMessage;
      do
      {
        m_stopMessage = RandomJuliaChildPhrase.GetRandomPhrase();
      } while (m_stopMessage.Equals(m_startMessage) || m_stopMessage.Equals(current));
    }

    #region Overrides of SE_Stats

    /// <inheritdoc />
    public override void Setup(Character character)
    {
      SetMessages();
      base.Setup(character);
    }

    #endregion
  }

  /// <summary>
  /// Random Julia Child Phrases
  /// </summary>
  public static class RandomJuliaChildPhrase
  {
    /// <summary>
    /// List of Julia Child Phrases
    /// </summary>
    private static readonly List<string> PhrasesList = new List<string>
    {
      "No one is born a great cook, one learns by doing. - Julia Child"
      , "You don’t have to cook fancy or complicated masterpieces, just good food from fresh ingredients. - Julia Child"
      , "People who love to eat are always the best people. - Julia Child"
      , "Cooking well doesn’t mean cooking fancy. - Julia Child"
      , "The only time to eat diet food is while you’re waiting for the steak to cook. - Julia Child"
      , "Until I discovered cooking, I was never really interested in anything. - Julia Child"
      , "You are the butter to my bread, and the breath to my life. - Julia Child"
      , "Fat gives things flavor. - Julia Child"
      , "If you’re afraid of butter, use cream. - Julia Child"
      , "I was 32 when I started cooking; up until then, I just ate. - Julia Child"
      , "A party without cake is just a meeting. - Julia Child"
      , "The only real stumbling block is fear of failure. In cooking you’ve got to have a what-the-hell attitude. - Julia Child"
      , "In France, cooking is a serious art form and a national sport. - Julia Child"
      , "You are the BOSS of that dough. - Julia Child"
      , "with enough butter anything is good. - Julia Child"
      , "Usually, one’s cooking is better than one thinks it is. - Julia Child"
    };

    /// <summary>
    /// Gets a random Julia Child Phrase.
    /// </summary>
    /// <returns>Random Julia Child Phrase</returns>
    public static string GetRandomPhrase()
    {
      return PhrasesList[Random.Range(0, PhrasesList.Count -1)];
    }
  }
}
