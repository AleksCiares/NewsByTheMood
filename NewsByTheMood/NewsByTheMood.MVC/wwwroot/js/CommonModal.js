class CommonModal{
    #modal;
    #buttonsContainer;
    #buttons = [];

	constructor(modalLabelText, modalBodyText, preventElementId) {
        const modalHtml = `<div class="modal fade" id="commonModal" tabindex="-1" aria-labelledby="commonModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="commonModalLabel">${modalLabelText}</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body" id="commonModalBody">
                                    ${modalBodyText}
                                </div>
                                <div class="modal-footer" id="commonModalButtonContainer">
                                </div>
                            </div>
                        </div>
                    </div>`;
        document.body.insertAdjacentHTML("beforeend", modalHtml);
        this.#modal = new bootstrap.Modal(document.getElementById("commonModal"), {});
        this.#buttonsContainer = document.getElementById("commonModalButtonContainer");

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
        cancelButton.id = "commonModalButtonCancel";
        cancelButton.type = "button";
        cancelButton.className = `btn ${buttonCssClass}`;
        cancelButton.setAttribute("data-bs-dismiss", "modal");
        cancelButton.innerText = buttonText;

        this.#buttons.push(cancelButton);
        this.#buttonsContainer.appendChild(cancelButton);
    }

    setConfirmButton(buttonText, buttonCssClass, eventHandler) {
        const button = document.createElement("button");
        button.id = `commonModalButton_${this.#buttons.length}`;
        button.type = "button";
        button.className = `btn ${buttonCssClass}`;
        button.innerText = buttonText;

        if (eventHandler) {
            button.addEventListener("click", (e) => {
                this.#modal.hide();
                eventHandler();
            });
        }

        this.#buttons.push(button);
        this.#buttonsContainer.appendChild(button);
    }
}