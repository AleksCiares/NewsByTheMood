class CommonModal{
	#modalHtml;
    #modal;

	constructor(modalLabelText, modalBodyText, modalCancelText, modalSubmitText, preventElementId, submitEventHandler) {
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
                                <div class="modal-footer">
                                    <button id="commonModalCancel" type="button" class="btn btn-secondary" data-bs-dismiss="modal">${modalCancelText}</button>
                                    <button id="commonModalSubmit" type="button" class="btn btn-danger">${modalSubmitText}</button>
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

        let submitButton = document.getElementById("commonModalSubmit");
        submitButton.addEventListener("click", (e) => {
            e.preventDefault();
            if (submitEventHandler) {
                submitEventHandler();
            }
            this.#modal.hide();
        });
    }
}