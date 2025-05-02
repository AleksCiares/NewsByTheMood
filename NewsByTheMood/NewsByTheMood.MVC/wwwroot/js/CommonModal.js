class CommonModal{
    #modalId;
    #modal;
    #buttonsContainer;
    #buttonsCount = 0;

    constructor(modalLabelText, modalBodyText, preventElementId) {
        const randomNumber = Math.floor(Math.random() * (1000000 - 10 + 1) + 10);

        this.#modalId = "commonModal_" + randomNumber;
        const modalLabelId = "commonModalLabel_" + randomNumber;

        const modalHtml = `<div class="modal fade" id="${this.#modalId}" tabindex="-1" aria-labelledby="${modalLabelId}" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="${modalLabelId}">${modalLabelText}</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-message modal-body">
                                        ${modalBodyText}
                                    </div>
                                    <div class="modal-buttons-container modal-footer">
                                    </div>
                                </div>
                            </div>
                        </div>`;

        document.body.insertAdjacentHTML("beforeend", modalHtml);
        this.#modal = new bootstrap.Modal(document.getElementById(this.#modalId), {});
        this.#buttonsContainer = document.querySelector(`#${this.#modalId} .modal-buttons-container`);

        const preventElement = document.getElementById(preventElementId);
        if (preventElement) {
            preventElement.addEventListener("click", (e) => {
                e.preventDefault();
                this.#modal.show();
            });
        }
    }

    setCancelButton(buttonText, buttonCssClass) {
        const cancelButton = document.createElement("button");
        cancelButton.type = "button";
        cancelButton.className = `cancel-button_${this.#buttonsCount} btn ${buttonCssClass}`;
        cancelButton.setAttribute("data-bs-dismiss", "modal");
        cancelButton.innerText = buttonText;

        this.#buttonsContainer.appendChild(cancelButton);

        this.#buttonsCount++;
    }

    setConfirmButton(buttonText, buttonCssClass, eventHandler) {
        const button = document.createElement("button");
        button.type = "button";
        button.className = `confirm-button_${this.#buttonsCount} btn ${buttonCssClass}`;
        button.innerText = buttonText;

        if (eventHandler) {
            button.addEventListener("click", (e) => {
                this.#modal.hide();
                eventHandler();
            });
        }

        this.#buttonsContainer.appendChild(button);

        this.#buttonsCount++;
    }
}