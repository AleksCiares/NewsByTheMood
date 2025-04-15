class CommonModal{
	#modalHtml;
    #modal;
    #buttons = [];

	constructor(modalLabelText, modalBodyText, preventElementId) {
        this.#modalHtml = `<div class="modal fade" id="commonModal" tabindex="-1" aria-labelledby="commonModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="commonModalLabel">${modalLabelText}</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body" id="commonModalBody">
                                    ${modalBodyText}
                                </div>
                                <div id="commonModalButtonContainer" class="modal-footer">
                                </div>
                            </div>
                        </div>
                    </div>`;

        document.body.insertAdjacentHTML("beforeend", this.#modalHtml);
        this.#modal = new bootstrap.Modal(document.getElementById("commonModal"), {});

        let preventElement = document.getElementById(preventElementId);
        if (preventElement) {
            preventElement.addEventListener("click", (e) => {
                e.preventDefault();
                this.#modal.show();
            });
        }
    }

    setCancelButton(buttonText, buttonCssClass) {
        let buttonContaier = document.getElementById("commonModalButtonContainer");
        buttonContaier.insertAdjacentHTML("beforeend", `<button id="commonModalButtonCancel" type="button" class="btn ${buttonCssClass}" data-bs-dismiss="modal">${buttonText}</button>`);
    }

    setConfirmButton(buttonText, buttonCssClass, eventHandler) {
        let buttonContaier = document.getElementById("commonModalButtonContainer");

        let buttonId = `commonModalButton${this.#buttons.length}`;
        this.#buttons.push(buttonId);
        buttonContaier.insertAdjacentHTML("beforeend", `<button id="${this.#buttons[this.#buttons.length - 1]}" type="button" class="btn ${buttonCssClass}" >${buttonText}</button>`);

        let button = document.getElementById(this.#buttons[this.#buttons.length - 1]);
        if (eventHandler) {
            button.addEventListener("click", (e) => {
                e.preventDefault();
                eventHandler();
                this.#modal.hide();
            });
        }
    }
}