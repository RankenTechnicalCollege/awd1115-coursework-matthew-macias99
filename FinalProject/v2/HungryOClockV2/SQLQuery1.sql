UPDATE Restaurants
SET Slug = LOWER(REPLACE(Name, ' ', '-'))
WHERE Slug IS NULL OR Slug = '';