
document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'articleForm',
		'articleFormSubmit',
		'Success update article. ',
		undefined,
		'Error update article. ',
		showGeneralNotifyMessage
	);

	const updateModal = new CommonModal(
		'Update article',
		'Are you sure to update this article? These articles will be restored to their original state.',
		'reloadArticleButton'
	);

	updateModal.setCancelButton('Cancel', 'btn-secondary');
	updateModal.setConfirmButton('Confirm', 'btn-primary', async () => {
		const reloadButton = document.getElementById('reloadArticleButton');
		const reloadSpinner = document.getElementById('reloadSpinner');

		reloadSpinner.classList.remove('d-none');
		reloadButton.disabled = true;

		await sendAsyncRequest(reloadButton.href, 'POST', window.location.pathname, showGeneralNotifyMessage);

		reloadSpinner.classList.add('d-none');
		reloadButton.disabled = false;
	});

	const deleteModal = new CommonModal(
		'Delete article',
		'Are you sure to delete this article?',
		'deleteArticleButton'
	);

	deleteModal.setCancelButton('Cancel', 'btn-secondary');
	deleteModal.setConfirmButton('Confirm', 'btn-danger', async () => {
		const href = document.getElementById('deleteArticleButton').href;
		await sendAsyncRequest(href, 'POST', '/settings/articles/', showGeneralNotifyMessage);
	});

	const deleteCommentsModal = new CommonModal(
		'Confirm deletion',
		'Are you sure to delete the selected comments?',
		'deleteSelectedComments'
	);

	deleteCommentsModal.setCancelButton('Cancel', 'btn-secondary');
	deleteCommentsModal.setConfirmButton('Confirm', 'btn-danger', async () => {
		const commentsIds = [];
		const formData = new URLSearchParams();
		document.querySelectorAll('.select-comment:checked').forEach(checkbox => {
			commentsIds.push(checkbox.value);
			formData.append('ids', checkbox.value);
		});

		const result = await sendAsyncRequest(
		'/settings/articles/deletecommentsrange/',
		'POST',
		undefined,
		showGeneralNotifyMessage,
		formData,
		{
			"X-Requested-With": "XMLHttpRequest",
			"Content-Type": "application/x-www-form-urlencoded"
		});

		if (result) {
			commentsIds.forEach(id => {
				const commentBody = document.querySelector(`[data-comment-id="${id}"] .comment__body`);
				commentBody.innerHTML = 'Deleted by moderator';
				commentBody.classList.add('text-muted');
			});

			document.querySelectorAll('.select-comment:checked').forEach(checkbox => {
				checkbox.checked = false;
			});
		}
	});
});
