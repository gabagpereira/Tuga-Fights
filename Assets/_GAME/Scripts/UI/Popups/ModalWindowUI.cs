using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace MemeFight.UI.Popups
{
    public class ModalWindowUI : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] LocalizedTextUI _displayText;

        [Header("Buttons")]
        [SerializeField] ButtonUI _confirmButton;
        [SerializeField] ButtonUI _declineButton;

        [Space(10)]
        [SerializeField] UnityEvent _onOpen;
        [SerializeField] UnityEvent _onClose;

        Action _onConfirmCallback;
        Action _onDeclineCallback;

        public bool IsOpen => gameObject.activeInHierarchy;

        public event UnityAction OnClose;

        void OnDisable()
        {
            Debug.Log("Modal window disabled");
        }

        void Awake()
        {
            _confirmButton.OnClicked += HandleConfirmButtonClicked;
            _declineButton.OnClicked += HandleDeclineButtonClicked;
        }

        void HandleConfirmButtonClicked() => OnButtonClicked(_onConfirmCallback);
        void HandleDeclineButtonClicked() => OnButtonClicked(_onDeclineCallback);

        void OnButtonClicked(Action action)
        {
            action?.Invoke();
            Close();
        }

        public void Configure(LocalizedString textString, ButtonConfig confirmBtnConfig, ButtonConfig declineBtnConfig)
        {
            _displayText.UpdateTextString(textString);

            _onConfirmCallback = confirmBtnConfig.callback.Invoke;
            _onDeclineCallback = declineBtnConfig.callback.Invoke;

            _confirmButton.Configure(confirmBtnConfig.displayNameString, confirmBtnConfig.icon, confirmBtnConfig.isEnabled);
            _declineButton.Configure(declineBtnConfig.displayNameString, declineBtnConfig.icon, declineBtnConfig.isEnabled);

            _confirmButton.gameObject.SetActive(confirmBtnConfig.HasCallback);
            _declineButton.gameObject.SetActive(declineBtnConfig.HasCallback);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _onOpen.Invoke();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
            _onClose.Invoke();
        }
    }
}
