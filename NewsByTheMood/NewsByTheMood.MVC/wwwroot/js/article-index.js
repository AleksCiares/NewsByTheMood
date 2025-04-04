document.addEventListener("DOMContentLoaded", () => {
    function toggleDeleteButton() {
        document.getElementById('deleteSelectedArticles').disabled = (document.querySelectorAll('.selectArticle:checked').length === 0);
    }

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
        'Delete Articles',
        'Are you sure you want to delete the selected articles?',
        'Cancel',
        'Delete',
        'deleteSelectedArticles',
        () => {
            let form = document.querySelector('form');
            form.submit();
        }
    );
});

