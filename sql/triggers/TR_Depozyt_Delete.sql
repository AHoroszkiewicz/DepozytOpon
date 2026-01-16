CREATE TRIGGER trg_Depozyt_Delete
ON Depozyt
AFTER DELETE
AS
BEGIN
    INSERT INTO DepozytHistory
    (
        NumerBOX,
        ImieNazwisko,
        NumerTelefonu,
        MarkaPojazdu,
        RejestracjaPojazdu,
        OponaId,
        Ilosc,
        DataPrzyjecia,
        Notatka,
        DataEdycji,
        EdytowanoPrzez,
        UtworzonoPrzez
    )
    SELECT 
        d.NumerBOX,
        d.ImieNazwisko,
        d.NumerTelefonu,
        d.MarkaPojazdu,
        d.RejestracjaPojazdu,
        d.OponaId,
        d.Ilosc,
        d.DataPrzyjecia,
        d.Notatka,
        d.DataEdycji,
        d.EdytowanoPrzez,
        d.UtworzonoPrzez
    FROM deleted d
END
