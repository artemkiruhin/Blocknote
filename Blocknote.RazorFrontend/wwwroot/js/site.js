document.addEventListener('DOMContentLoaded', () => {
    const addNoteBtn = document.querySelector('.add-note-btn');
    const modal = document.querySelector('.modal');
    const modalCancel = document.querySelector('.modal-cancel');
    const modalSave = document.querySelector('.modal-save');
    const noteView = document.querySelector('.note');
    const closeNoteBtn = document.querySelector('.close-note-btn');
    const container = document.querySelector('.container');

    
    addNoteBtn.addEventListener('click', () => {
        modal.classList.remove('hidden');
    });

    
    modalCancel.addEventListener('click', () => {
        modal.classList.add('hidden');
    });

   
    closeNoteBtn.addEventListener('click', () => {
        noteView.classList.add('hidden');
    });

   
    modalSave.addEventListener('click', () => {
        const title = document.querySelector('.modal-input').value;
        const content = document.querySelector('.modal-textarea').value;

        if (title && content) {
            modal.classList.add('hidden');
        }
    });
    
    // Обработка контекстного меню
    function attachNoteCardListeners(noteCard, content) {
        const menu = noteCard.querySelector('.note-card-menu');

        // Открытие заметки по клику
        noteCard.querySelector('.note-card-content').addEventListener('click', () => {
            openNote(noteCard.querySelector('.note-card-title').textContent, content);
        });

        // Правый клик по карточке
        noteCard.addEventListener('contextmenu', (e) => {
            e.preventDefault();
            e.stopPropagation();
            showMenu(e, menu);
        });

        // Клик по пунктам меню
        menu.addEventListener('click', (e) => {
            e.stopPropagation();
            const action = e.target.dataset.action;

            switch(action) {
                case 'open':
                    openNote(noteCard.querySelector('.note-card-title').textContent, content);
                    break;
                case 'edit':
                    // Здесь будет функционал редактирования
                    break;
                case 'delete':
                    if (confirm('Вы уверены, что хотите удалить эту заметку?')) {
                        noteCard.remove();
                    }
                    break;
            }

            hideAllMenus();
        });
    }

    // Показ контекстного меню
    function showMenu(e, menu) {
        hideAllMenus();

        const rect = menu.getBoundingClientRect();
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;

        // Рассчитываем координаты с учетом границ окна
        let x = e.clientX;
        let y = e.clientY;

        // Проверяем правую границу
        if (x + rect.width > windowWidth) {
            x = windowWidth - rect.width;
        }

        // Проверяем нижнюю границу
        if (y + rect.height > windowHeight) {
            y = windowHeight - rect.height;
        }

        // Устанавливаем позицию меню
        menu.style.left = `${x}px`;
        menu.style.top = `${y}px`;

        // Показываем меню
        requestAnimationFrame(() => {
            menu.classList.add('visible');
        });
    }

    // Скрытие всех открытых меню
    function hideAllMenus() {
        document.querySelectorAll('.note-card-menu.visible').forEach(menu => {
            menu.classList.remove('visible');
        });
    }

    // Открытие заметки
    function openNote(title, content) {
        noteView.querySelector('.note-title').textContent = title;
        noteView.querySelector('.note-content').innerHTML = `<p>${content}</p>`;
        noteView.classList.remove('hidden');
    }

    // Закрытие меню при клике вне его
    document.addEventListener('click', (e) => {
        if (!e.target.closest('.note-card-menu')) {
            hideAllMenus();
        }
    });

    // Закрытие меню при правом клике вне карточки
    document.addEventListener('contextmenu', (e) => {
        if (!e.target.closest('.note-card')) {
            e.preventDefault();
            hideAllMenus();
        }
    });

    // Инициализация существующих карточек
    document.querySelectorAll('.note-card').forEach(noteCard => {
        attachNoteCardListeners(noteCard, 'Содержимое заметки...');
    });
});