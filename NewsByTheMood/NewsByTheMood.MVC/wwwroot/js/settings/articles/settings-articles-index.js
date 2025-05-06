function toggleDeleteButton() {
    document.getElementById('deleteSelectedArticles').disabled =
        (document.querySelectorAll('.selectArticle:checked').length === 0);
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById('selectAllArticles').addEventListener('change', el => {
        let checkboxes = document.querySelectorAll('.selectArticle');
        for (let checkbox of checkboxes) {
            checkbox.checked = el.currentTarget.checked;
        }
        toggleDeleteButton();
    });
    document.querySelectorAll('.selectArticle').forEach(checkbox => {
        checkbox.addEventListener('change', toggleDeleteButton);
    });
    toggleDeleteButton();

    let modal = new CommonModal(
        'Confirm deletion',
        'Are you sure to delete the selected articles?',
        'deleteSelectedArticles'
    );
    modal.setCancelButton('Cancel', 'btn-secondary');
    modal.setConfirmButton('Confirm', 'btn-danger', () => {
        let form = document.getElementById('deleteArticlesForm');
        form.submit();
    });
});