export default class Note {
    constructor(id, title, subtitle, content) {
        this.id = id;
        this.title = title;
        this.subtitle = subtitle ?? "";
        this.content = content ?? "";
    }
}