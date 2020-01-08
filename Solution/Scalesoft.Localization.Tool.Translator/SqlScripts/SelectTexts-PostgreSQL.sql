SELECT bt."Id", c."Name" as "CultureName", ds."Name" as "ScopeName", bt."Name" as "Key", bt."Text"
	FROM public."BaseText" bt
	inner join "Culture" c on (bt."Culture" = c."Id")
	inner join "DictionaryScope" ds on (bt."DictionaryScope" = ds."Id")
	order by bt."DictionaryScope" asc, bt."Name" asc, bt."Culture";