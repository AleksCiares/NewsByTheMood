document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'sourceForm',
		'sourceFormSubmit',
		'Success update source. ',
		undefined,
		'Error update source. ',
		showGeneralNotifyMessage,
	);

	const updateModal = new CommonModal(
		'Load articles',
		'Are you sure to load articles from source manually?',
		'loadArticlesButton'
	);

	updateModal.setCancelButton('Cancel', 'btn-secondary');
	updateModal.setConfirmButton('Confirm', 'btn-primary', async () => {
		const reloadButton = document.getElementById('loadArticlesButton');
		const reloadSpinner = document.getElementById('reloadSpinner');

		reloadSpinner.classList.remove('d-none');
		reloadButton.disabled = true;

		const result = await sendAsyncRequest(reloadButton.href, 'POST', undefined, showGeneralNotifyMessage);

		reloadSpinner.classList.add('d-none');
		reloadButton.disabled = false;

		if (result) {
			showGeneralNotifyMessage('Success load articles. For more information watch logs.', 'success');
		}
	});
});